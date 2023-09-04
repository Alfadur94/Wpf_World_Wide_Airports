using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_World_Wide_Airports
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Airport> airports = new List<Airport>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Check if csv file exists in Programfolder
            string filename = "airports.csv";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, filename);

            //If csv file does not exist give user a message and the option to download it
            if (!System.IO.File.Exists(path))
            {
                MessageBoxResult result = MessageBox.Show("The airports.csv file is missing. Do you want to download it?", "File missing", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string url = "https://davidmegginson.github.io/ourairports-data/airports.csv";
                        string filname = "airports.csv";
                        string pathe = System.IO.Path.Combine(Environment.CurrentDirectory, filname);
                        System.Net.WebClient webClient = new System.Net.WebClient();
                        webClient.DownloadFile(url, pathe);                        
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
            {
                //If csv file exists, load it into a list of Airport objects with CsvHelper
                //Read csv file into list of Airport objects
                using (var reader = new System.IO.StreamReader(path))
                using (var csv = new CsvHelper.CsvReader(reader, culture: System.Globalization.CultureInfo.InvariantCulture))
                {

                    airports = csv.GetRecords<Airport>().ToList();
                }

                //Load the continets into the combobox
                var continents = airports.Select(a => a.continent).Distinct().OrderBy(a => a);
                foreach (var continent in continents)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = continent;
                    cbContinent.Items.Add(item);
                }

                //If the user selects a continent, activate the country combobox
                cbContinent.SelectionChanged += (s, a) =>
                {
                    cbCountry.IsEnabled = true;
                };

            }
        }

        private void cbContinent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {               
            
            //Load the countries into the combobox where the continent is the same as the selected continent
            var countries = airports.Where(a => a.continent == cbContinent.SelectedItem.ToString()).Select(a => a.iso_country).Distinct().OrderBy(a => a);
            foreach (var country in countries)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = country;
                cbCountry.Items.Add(item);
            }

        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Load csv file from URL into Programfolder
            //string url = "https://davidmegginson.github.io/ourairports-data/airports.csv";
            //string filename = "airports.csv";
            //string path = System.IO.Path.Combine(Environment.CurrentDirectory, filename);
            //System.Net.WebClient webClient = new System.Net.WebClient();
            //webClient.DownloadFile(url, path);
        }
    }
}
