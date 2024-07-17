using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library;
using ModelesLibrary;
using Syncfusion.UI.Xaml.Maps;
using Syncfusion.Windows.Controls.Layout;

namespace CountryInfo
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {
        public Details(Country country)
        {
            InitializeComponent();
            SfAccordion accordion = new SfAccordion();
            //Adding SfAccordion as window content
            this.Content = accordion;
            InitAccordion(country);
        }


        private void InitAccordion(Country country)
        {

            //Creates the accordion item
            SfAccordion accordion = new SfAccordion();
            accordion.FontSize = 20;

            //String to hold the info of each item, after applying to the accordion its clean to avoid spiling information.
            string tempContent = "";

            //Creating the individual accordion items and its contents
            //Setting header for SfAccordionItem
            SfAccordionItem accordionItem1 = new SfAccordionItem();
            accordionItem1.Header = "Info Basica";
            
            tempContent = $"Nome comum: {country.Name.GetCommonString} {Environment.NewLine}";
            tempContent += $"Nome Oficial: {country.Name.GetOfficialString}";

            accordionItem1.Content = tempContent;

            tempContent = "";

            SfAccordionItem accordionItem2 = new SfAccordionItem();
            accordionItem2.Header = "Bandeira";


            Image img = new Image();
            img.Source = new BitmapImage(new Uri(country.Flags.GetLocalRefString));
            img.Width = 400;
            accordionItem2.Content = img;
           


            SfAccordionItem accordionItem3 = new SfAccordionItem();
            accordionItem3.Header = "Capital";

            //Checks if Country has more than 1 capital
            if (country.Capital!=null && country.Capital.Count>1)
            {
                int countCapitals = 1;
                foreach (string capital in country.Capital)
                {
                    tempContent += $"Capital {countCapitals}: {capital} {Environment.NewLine}";
                    countCapitals++;
                }
            }
            else
            {
               
               tempContent= $"Capital: {country.GetCapitalString.First()}";
            }
            
            accordionItem3.Content = tempContent;

            tempContent = "";

            SfAccordionItem accordionItem4 = new SfAccordionItem();
            accordionItem4.Header = "Região";

            tempContent += $"Região: {country.GetRegionString} {Environment.NewLine}";
            tempContent += $"Sub-Região: {country.GetSubRegionString}";

            accordionItem4.Content = tempContent;

            tempContent = "";

            SfAccordionItem accordionItem5 = new SfAccordionItem();
            accordionItem5.Header = "População";

            tempContent += $"População: {country.GetPopulationString} Abitantes.";

            accordionItem5.Content = tempContent;

            tempContent = "";

            SfAccordionItem accordionItem6 = new SfAccordionItem();
            accordionItem6.Header = "Gini";

            if (country.LastGine == null)
            {
                foreach (string gini in country.GetGiniList)
                {
                    tempContent += gini;
                }
            }
            else {
                tempContent += country.LastGine;
            }


            accordionItem6.Content = tempContent;



            SfAccordionItem accordionItem7 = new SfAccordionItem();
            accordionItem7.Header ="Mapa";
            SfMap syncMap = new SfMap();


            ImageryLayer MapData = new ImageryLayer();
            Point point = new Point();
            point.X = country.GetLatlng[0];
            point.Y = country.GetLatlng[1];

            MapData.Center = point;
            syncMap.Layers.Add(MapData);

            syncMap.ZoomLevel = 5;
            accordionItem7.Content = syncMap;




            //Adding the items to the accordion
            accordion.Items.Add(accordionItem1);
            accordion.Items.Add(accordionItem2);
            accordion.Items.Add(accordionItem3);
            accordion.Items.Add(accordionItem4);
            accordion.Items.Add(accordionItem5);
            accordion.Items.Add(accordionItem6);
            accordion.Items.Add(accordionItem7);

            //Adding the accordion to the view
            this.Content = accordion;


        }

       


    }
}
