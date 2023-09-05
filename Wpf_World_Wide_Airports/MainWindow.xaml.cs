using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Http;
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

        private async void loadCSV_Click(object sender, RoutedEventArgs e)
        {
            string fileUrl = "https://davidmegginson.github.io/ourairports-data/airports.csv";
            string filename = "airports.csv";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, filename);

            if (File.Exists(path))
            {
                var confirmResult = MessageBox.Show("Die Datei 'airports.csv' existiert bereits. Möchten Sie sie überschreiben?", "Bestätigung erforderlich", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.No)
                {
                    return;
                }
            }

            bool downloadSuccess = await DownloadFileAsync(fileUrl, path);

            if (downloadSuccess)
            {
                MessageBox.Show("Die Datei 'airports.csv' wurde erfolgreich heruntergeladen und gespeichert.");
            }
            else
            {
                MessageBox.Show("Fehler beim Herunterladen der Datei. Bitte versuchen Sie es erneut.");
            }
        }

        private async Task<bool> DownloadFileAsync(string fileUrl, string path)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(fileUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        using (var fileStream = File.Create(path))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Fehler beim Herunterladen der Datei: {ex.Message}");
            }
            return false;
        }

        private void openCSV_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "airports.csv";
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";

            if (dlg.ShowDialog() == true)
            {
                string selectedFilePath = dlg.FileName;
                string destinationFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "airports.csv");

                if (File.Exists(destinationFilePath))
                {
                    MessageBoxResult result = MessageBox.Show("Die Datei existiert bereits. Möchten Sie sie überschreiben?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                try
                {
                    File.Copy(selectedFilePath, destinationFilePath, true);

                    MessageBox.Show("CSV-Datei wurde in den Projektordner kopiert.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Es ist ein Fehler aufgetreten: {ex.Message}");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string filename = "airports.csv";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, filename);
            if (File.Exists(path))
            {
                   DateTime lastModified = File.GetLastWriteTime(path);
                if (lastModified < DateTime.Now.AddDays(-2))
                {
                    MessageBoxResult result = MessageBox.Show("Die Datei 'airports.csv' ist älter als 2 Tage. Möchten Sie sie erneut herunterladen?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        loadCSV_Click(sender, e);
                    }
                    
                }
            }

            using var reader = new StreamReader(path);
            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                  airports = csv.GetRecords<Airport>().ToList();
            }

            //Dictionary<string, string> continentTranslations = new Dictionary<string, string>()
            //{
            //    {"AF","Afrika"},
            //    {"AN","Antarktis"},
            //    {"AS","Asien"},
            //    {"EU","Europa"},
            //    {"NA","Nordamerika"},
            //    {"OC","Ozeanien"},
            //    {"SA","Südamerika"}
            //};

            var continents = airports.Select(a => a.continent).Distinct().ToList();
            continents.Sort();
            foreach (var continent in continents)
            {
                //string translatedContinent = continentTranslations.ContainsKey(continent) ? continentTranslations[continent] : continent;
                cbContinent.Items.Add(continent);
            }
        }

        private void cbContinent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbContinent.SelectedItem != null)
            {
                cbCountry.IsEnabled = true;
                cbCountry.Items.Clear();
                var countries = airports.Where(a => a.continent == cbContinent.SelectedItem.ToString()).Select(a => a.iso_country).Distinct().ToList();
                countries.Sort();
                foreach (var country in countries)
                {
                    cbCountry.Items.Add(country);
                }
            }
        }

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null)
            {
                cbType.IsEnabled = true;
                cbType.Items.Clear();
                var types = airports.Where(a => a.iso_country == cbCountry.SelectedItem.ToString()).Select(a => a.type).Distinct().ToList();
                types.Sort();
                foreach (var type in types)
                {
                    cbType.Items.Add(type);
                }
            }
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedItem != null)
            {
                cbAirport.IsEnabled = true;
                cbAirport.Items.Clear();
                var airportsFiltered = airports.Where(a => a.iso_country == cbCountry.SelectedItem.ToString() && a.type == cbType.SelectedItem.ToString()).Select(a => a.name).Distinct().ToList();
                airportsFiltered.Sort();
                foreach (var airport in airportsFiltered)
                {
                    cbAirport.Items.Add(airport);
                }
            }
        }

        private void cbAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}