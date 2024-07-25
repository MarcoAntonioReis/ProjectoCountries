using Library;
using ModelesLibrary;
using Services;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CountryInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkService networkService;
        private ApiService apiService;
        private DataService dataService;
        private List<Country> Countries;
        private string FlagsPath;

        public MainWindow()
        {

            InitializeComponent();
            MainStatusEvents(1);

            networkService = new NetworkService();
            apiService = new ApiService();
            dataService = new DataService();

            FlagsPath = GetFlagsPath();
            BtnGetBannersSync.IsEnabled = false;

            LoadData();

        }


        /// <summary>
        /// Loads the data Grid with the data retrieved in the api services, it also creates a backup of the data retrieved to a local data base.
        /// </summary>
        public async Task LoadGridAsync()
        {

            await LoadGridMainDataAsync();
            if (Countries != null)
            {
                SetBannersLocalRef();
                CheckBannersExists();
                SetFlagsOnGrid();
                SaveToLocal();
            }
            else
            {
                await MainStatusEvents(-2);
            }



        }

        /// <summary>
        /// TODO melhorar
        /// </summary>
        public void SaveToLocal()
        {
            dataService.DeleteData();
            dataService.SaveDataAsync(Countries);
        }

        /// <summary>
        /// Sets some of the main style of the grid
        /// </summary>
        public void SetDataGridStyle()
        {

            DataGridCountries.RowHeight = 75;
            DataGridCountries.FontSize = 20;

        }
        /// <summary>
        /// Main method for the app, it contains main logic, it checks the internet connection and acts in accord to the available connections and data. 
        /// </summary>
        private async void LoadData()
        {
            await MainStatusEvents(2);
            bool load;

            var connection = networkService.CheckConnection();
            SetDataGridStyle();
            if (!connection.IsSuccess)
            {
                LoadLocalCountries();
                load = false;
            }
            else
            {
                await LoadGridAsync();
                load = true;
                BtnGetBannersSync.IsEnabled = true;
            }

            //For safety it checks if something went wrong by checking if the list was correctly initialize, because the error messages are dealt when a error may occurred and the reason may change it is not processed here
            if (Countries != null)
            {
                List<string> RegionsList = new List<string>();
                //The empty string is to allow a rest of the search parameters
                RegionsList.Add(" ");
                RegionsList.AddRange(Countries.Select(x => x.Region).Distinct().ToList());               
                ComboRegions.ItemsSource = RegionsList;

                await MainStatusEvents(3);
                await MainStatusEvents(4);
                if (load)
                {
                    TxtStatus.Text = string.Format("Dados actualizados da internet em {0:F}", DateTime.Now);
                }
                else
                {
                    TxtStatus.Text = string.Format("Dados carregados localmente");
                }
            }
            


        }

        /// <summary>
        /// method for the case of not being connected to the internet, and it retrieves countries from a local data Basse
        /// </summary>
        private async void LoadLocalCountries()
        {
            Countries = dataService.Getdata();
            Countries.OrderBy(x => x.Name.Common);
            if (Countries != null)
            {
                SetGridData();
                CheckBannersExists();
                SetFlagsOnGrid();
            }
            else
            {
                await MainStatusEvents(-1);
            }



        }

        /// <summary>
        /// Calls the api service to get the list of countries
        /// </summary>
        /// <returns></returns>
        private async Task LoadGridMainDataAsync()
        {

            var response = await apiService.GetResponseAsync("https://restcountries.com/", "/v3.1/all");


            Countries = (List<Country>)response.Result;

            Countries = Countries.OrderBy(x => x.Name.Common).ToList();

            SetGridData();
        }


        /// <summary>
        /// This method is used to add the column of the country name in the grid
        /// </summary>
        private void SetGridData()
        {
            DataGridCountries.AutoGenerateColumns = false;
            DataGridCountries.ItemsSource = Countries;


            DataGridTextColumn NameColumn = new DataGridTextColumn();
            NameColumn.Header = "Nome do Pais";
            Binding b1 = new Binding("Name.GetCommonString");
            //Because this property are readOnly the biding must be OneWay
            b1.Mode = BindingMode.OneWay;

            NameColumn.Binding = b1;
            NameColumn.Width = 300;

            //manual creates and sets the text Wrap style to allow the text to wrap if its long, and the text to be vertically centerer
            var textWrappingStyle = new Style(typeof(TextBlock));
            textWrappingStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
            textWrappingStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            NameColumn.ElementStyle = textWrappingStyle;


            DataGridCountries.Columns.Add(NameColumn);
            DataGridCountries.CanUserAddRows = false;
        }

        /// <summary>
        /// After the main data is loaded to the grid it will try to load the images, this can be a long process because it neds to convert the data.
        /// </summary>
        private async Task DownloadBanners()
        {
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();


            progress.ProgressChanged += ReportProgress;
            await apiService.DownloadBanners(progress, Countries, FlagsPath);

        }

        /// <summary>
        /// Method to go get the location of the flags directory in the running context.
        /// </summary>
        /// <returns></returns>
        private string GetFlagsPath()
        {
            string workingDirectory = Environment.CurrentDirectory;

            //Warning does not work in published
            string GenFlagsPath = $"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Flags";
            //string RelasePath = $"{System.AppDomain.CurrentDomain.BaseDirectory}/Flags";


            return @$"{GenFlagsPath}\";


        }


        /// <summary>
        /// Method to set downloaded images flags in the grid, by setting the column of the flags to the data grid
        /// </summary>
        private void SetFlagsOnGrid()
        {

            if (DataGridCountries.Columns.Count == 2)
            {
                DataGridCountries.Columns.RemoveAt(1);
            }

            //Creating a basic col to start
            DataGridTemplateColumn colBanner = new DataGridTemplateColumn();
            colBanner.Header = "Bandeira";

            //create a factoryImage of type Image to support the image format in the cell
            FrameworkElementFactory factoryImage = new FrameworkElementFactory(typeof(Image));

            //Defining the property that indicates the flag location, the Binding mode is necessary for the set value
            Binding b1 = new Binding("Flags.GetLocalRefString");
            //Because this property are readOnly the biding must be OneWay
            b1.Mode = BindingMode.OneWay;


            factoryImage.SetValue(Image.SourceProperty, b1);


            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factoryImage;
            colBanner.CellTemplate = cellTemplate1;

            //Manual adjustment of the col index to 0 to be the left most item
            colBanner.DisplayIndex = 0;
            colBanner.Width = 200;
            DataGridCountries.Columns.Add(colBanner);

        }


        /// <summary>
        /// Method for the click event of the download flags
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GetBannersSync_Click(object sender, RoutedEventArgs e)
        {
            BtnGetBannersSync.IsEnabled = false;
            await DownloadBanners();
            SetBannersLocalRef();
            SetFlagsOnGrid();
            BtnGetBannersSync.IsEnabled = true;
        }


        /// <summary>
        /// Method to check if all banners exits in the local folder, if it does not it will referee the country to the no image image
        /// </summary>
        private void CheckBannersExists()
        {
            //Warning does not work in published
            //Checks if the placeholder ims exist if not it creates one
            if (!File.Exists(@$"{FlagsPath}\noImg.png"))
            {
                string workingDirectory = Environment.CurrentDirectory;
                string ResourcesPath = @$"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Resources";

                apiService.CheckDirectory(GetFlagsPath());

                if (File.Exists($"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}//Resources//noImg.png"))
                {
                    File.Copy(@$"{ResourcesPath}\noImg.png", @$"{FlagsPath}\noImg.png");

                    //File.WriteAllBytes(@$"{workingDirectory}\flags\noImg.png", Properties.Resources.noImg);
                }

            }

            foreach (Country country in Countries)
            {

                string filepath = country.Flags.LocalRef;

                if (!File.Exists(filepath))
                {
                    country.Flags.LocalRef = @$"{FlagsPath}\noImg.png";
                }


            }


        }

        /// <summary>
        /// Creates and sets the local path of the images location for each country
        /// </summary>
        private void SetBannersLocalRef()
        {

            foreach (Country country in Countries)
            {
                if (country.Flags != null)
                {
                    country.Flags.LocalRef = @$"{FlagsPath}\{country.Flags.Png.Substring(country.Flags.Png.LastIndexOf('/') + 1)}";
                }
            }
        }


        /// <summary>
        /// Event to show the details of a country whin double click on the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Country country = (Country)DataGridCountries.SelectedItem;
            Details details = new Details(country);
            details.Show();
        }


        /// <summary>
        /// Method that is fired when a progress is made in the async download
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportProgress(object? sender, ProgressReportModel e)
        {
            dashBordProgress.Value = e.PercentageComplete;
            TxtStatus.Text = e.Status;
        }



        /// <summary>
        /// Method to handle the main status messages updates.
        /// The phases are between 1 and 4 and -1 is for fail case.
        /// </summary>
        /// <param name="phase"></param>
        /// <returns></returns>
        private async Task MainStatusEvents(int phase)
        {

            switch (phase)
            {
                case 1:
                    DashBordProgressMain.Value = 10;
                    TxtMainStatus.Text = "A inicializar elementos.";
                    await Task.Delay(1000);
                    break;

                case 2:
                    DashBordProgressMain.Value = 30;
                    TxtMainStatus.Text = "A recolher dados";
                    await Task.Delay(1000);
                    break;


                case 3:
                    DashBordProgressMain.Value = 90;
                    TxtMainStatus.Text = "A carregar dados";
                    await Task.Delay(1000);
                    break;

                case 4:
                    DashBordProgressMain.Value = 100;
                    TxtMainStatus.Text = "Dados carregados com sucesso";
                    break;
                case -1:
                    DashBordProgressMain.Value = 100;
                    TxtMainStatus.Text = "Não foi possivel carregar dados, tente outra vez conectado a internet.";
                    break;

                case -2:
                    DashBordProgressMain.Value = 100;
                    TxtMainStatus.Text = "Não foi possivel carregar dados mesmo com internet, tente mais tarde.";
                    break;


                default:
                    break;
            }








        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DataGridCountries.ItemsSource = null;
            DataGridCountries.ItemsSource = Search();
        }

        private void ComboRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridCountries.ItemsSource = null;
            DataGridCountries.ItemsSource = Search();
        }

        /// <summary>
        /// Method that collets the search paraments from the view, and narrows the items showed 
        /// </summary>
        /// <returns></returns>
        private List<Country> Search()
        {
            List<Country> SearchResult = new List<Country>();
            string RegionValue = ComboRegions.SelectedValue?.ToString();
            string NameValue = TxtSearch.Text;
            if (!string.IsNullOrWhiteSpace(RegionValue))
            {
                SearchResult = Countries.Where(x => x.Region == RegionValue).ToList();
                if (!string.IsNullOrWhiteSpace(NameValue))
                {
                    //To lower is used ignore the lower/upper case of letters in the serach
                    SearchResult = SearchResult.Where(x => x.Name.Common.ToLower().Contains(NameValue.ToLower())).ToList();
                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(NameValue))
                {
                    SearchResult = Countries.Where(x => x.Name.Common.ToLower().Contains(NameValue.ToLower())).ToList();
                }
                else
                {
                    SearchResult = Countries;
                }
            }

            return SearchResult;
        }

     
    }
}