using Library;
using Services;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.IO;
using System.Net;
using System;
using System.IO.Pipes;

namespace CountryInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NetworkService networkService;
        private ApiService apiService;
        private List<Country> Countries;
        private string FlagsPath;

        public MainWindow()
        {
            InitializeComponent();
            networkService = new NetworkService();
            apiService = new ApiService();
            //debuggin fasse remove as soon as posible
            var connection = networkService.CheckConnection();
            LoadGrid();
        }



        public async void LoadGrid()
        {
            FlagsPath = GetFlagsPath();

            DataGridCountries.RowHeight = 75;
            DataGridCountries.FontSize = 20;

            await LoadGridMainData();
            SetBannersLocalRef();
            CheckBannersExists();
            SetFlagsOnGrid();

        }



        private async Task LoadGridMainData()
        {

            var response = await apiService.GetResponseAsync("https://restcountries.com/", "/v3.1/all");


            Countries = (List<Country>)response.Result;

            Countries = Countries.OrderBy(x => x.Name.Common).ToList();

            DataGridCountries.AutoGenerateColumns = false;
            DataGridCountries.ItemsSource = Countries;


            DataGridTextColumn NameColumn = new DataGridTextColumn();
            NameColumn.Header = "Nome do Pais";
            NameColumn.Binding = new Binding("Name.GetCommonString");
            NameColumn.Width =467;
            DataGridCountries.Columns.Add(NameColumn);

          




        }


        /// <summary>
        /// After the main data is loaded to the grid it will try to load the images, this can be a long process because it neds to convert the data.
        /// </summary>
        private async Task DownloadBanners()
        {

            await apiService.DownloadBanners(Countries, FlagsPath);

        }

        /// <summary>
        /// Method go get the location of the flags directory
        /// </summary>
        /// <returns></returns>
        private string GetFlagsPath()
        {
            string workingDirectory = Environment.CurrentDirectory;

            string FlagsPath = $"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Flags";

            return @$"{FlagsPath}\";
        }


        private void SetFlagsOnGrid()
        {


            ///TODO if  the image doesnt exist place a placeholder image

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
            b1.Mode = BindingMode.Default;


            factoryImage.SetValue(Image.SourceProperty, b1);
            

            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factoryImage;            
            colBanner.CellTemplate = cellTemplate1;
                
            //Manual adjustment of the col index to 0 to be the left most item
            colBanner.DisplayIndex = 0;
            colBanner.Width = 200;
            DataGridCountries.Columns.Add(colBanner);



            
          



        }

        private async void GetBannersSync_Click(object sender, RoutedEventArgs e)
        {   
            await DownloadBanners();
            SetBannersLocalRef();
            SetFlagsOnGrid();

        }



        private void CheckBannersExists()
        {
            //Checks if the placeholder ims exist if not it creates one
            if (!File.Exists(@$"{FlagsPath}\noImg.png"))
            {
                string workingDirectory = Environment.CurrentDirectory;
                string ResourcesPath = @$"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Resources";

                if (File.Exists(@$"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Resources/noImg.png"))
                {
                    File.Copy(@$"{ResourcesPath}\noImg.png", @$"{FlagsPath}\noImg.png");
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


        private void SetBannersLocalRef()
        {

            //TODO Optimiar para não estar a sempra repetir o caminho
            foreach (Country country in Countries)
            {
                if (country.Flags != null)
                {

                    //TODO if img does not exist load a placeholder img
                    country.Flags.LocalRef = @$"{FlagsPath}\{country.Flags.Png.Substring(country.Flags.Png.LastIndexOf('/') + 1)}";

                }
            }
        }


    }
}