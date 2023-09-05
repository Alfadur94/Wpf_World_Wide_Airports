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

        Dictionary<string, string> continentTranslations = new Dictionary<string, string>()
            {
                {"AF","Afrika"},
                {"AN","Antarktis"},
                {"AS","Asien"},
                {"EU","Europa"},
                {"NA","Nordamerika"},
                {"OC","Ozeanien"},
                {"SA","Südamerika"}
            };

        Dictionary<string, string> countryTranlations = new Dictionary<string, string>()
        {
            {"AD","Andorra"},
            {"AE","Vereinigte Arabische Emirate"},
            {"AF","Afghanistan"},
            {"AG","Antigua und Barbuda"},
            {"AI","Anguilla"},
            {"AL","Albanien"},
            {"AM","Armenien"},
            {"AO","Angola"},
            {"AQ","Antarktis"},
            {"AR","Argentinien"},
            {"AS","Amerikanisch-Samoa"},
            {"AT","Österreich"},
            {"AU","Australien"},
            {"AW","Aruba"},
            {"AX","Åland"},
            {"AZ","Aserbaidschan"},
            {"BA","Bosnien und Herzegowina"},
            {"BB","Barbados"},
            {"BD","Bangladesch"},
            {"BE","Belgien"},
            {"BF","Burkina Faso"},
            {"BG","Bulgarien"},
            {"BH","Bahrain"},
            {"BI","Burundi"},
            {"BJ","Benin"},
            {"BL","Saint-Barthélemy"},
            {"BM","Bermuda"},
            {"BN","Brunei Darussalam"},
            {"BO","Bolivien"},
            {"BQ","Bonaire, Sint Eustatius und Saba"},
            {"BR","Brasilien"},
            {"BS","Bahamas"},
            {"BT","Bhutan"},
            {"BV","Bouvetinsel"},
            {"BW","Botsuana"},
            {"BY","Belarus"},
            {"BZ","Belize"},
            {"CA","Kanada"},
            {"CC","Kokosinseln"},
            {"CD","Kongo, Demokratische Republik"},
            {"CF","Zentralafrikanische Republik"},
            {"CG","Kongo"},
            {"CH","Schweiz"},
            {"CI","Côte d’Ivoire"},
            {"CK","Cookinseln"},
            {"CL","Chile"},
            {"CM","Kamerun"},
            {"CN","China"},
            {"CO","Kolumbien"},
            {"CR","Costa Rica"},
            {"CU","Kuba"},
            {"CV","Kap Verde"},
            {"CW","Curaçao"},
            {"CX","Weihnachtsinsel"},
            {"CY","Zypern"},
            {"CZ","Tschechien"},
            {"DE","Deutschland"},
            {"DJ","Dschibuti"},
            {"DK","Dänemark"},
            {"DM","Dominika"},
            {"DO","Dominikanische Republik"},
            {"DZ","Algerien"},
            {"EC", "Ecuador"},
            {"EE","Estonia"},
            {"EG","Ägypten"},
            {"EH","West Sahara"},
            {"ER","Eritrea"},
            {"ES","Spanien"},
            {"ET","Äthiopien"},
            {"FI","Finnland"},
            {"FJ","Fidschi"},
            {"FK","Falklandinseln"},
            {"FM","Mikronesien"},
            {"FO","Färöer"},
            {"FR","Frankreich"},
            {"GA","Gabun"},
            {"GB","Vereinigtes Königreich"},
            {"GD","Grenada"},
            {"GE","Georgien"},
            {"GF","Französisch-Guayana"},
            {"GG","Guernsey"},
            {"GH","Ghana"},
            {"GI","Gibraltar"},
            {"GL","Grönland"},
            {"GM","Gambia"},
            {"GN","Guinea"},
            {"GP","Guadeloupe"},
            {"GQ","Äquatorialguinea"},
            {"GR","Griechenland"},
            {"GS","Südgeorgien und die Südlichen Sandwichinseln"},
            {"GT","Guatemala"},
            {"GU","Guam"},
            {"GW","Guinea-Bissau"},
            {"GY","Guyana"},
            {"HK","Hongkong"},
            {"HM","Heard und die McDonaldinseln"},
            {"HN","Honduras"},
            {"HR","Kroatien"},
            {"HT","Haiti"},
            {"HU","Ungarn"},
            {"ID","Indonesien"},
            {"IE","Irland"},
            {"IL","Israel"},
            {"IM","Isle of Man"},
            {"IN","Indien"},
            {"IO","Britisches Territorium im Indischen Ozean"},
            {"IQ","Irak"},
            {"IR","Iran"},
            {"IS","Island"},
            {"IT","Italien"},
            {"JE","Jersey"},
            {"JM","Jamaika"},
            {"JO","Jordanien"},
            {"JP","Japan"},
            {"KE","Kenia"},
            {"KG","Kirgisistan"},
            {"KH","Kambodscha"},
            {"KI","Kiribati"},
            {"KM","Komoren"},
            {"KN","St. Kitts und Nevis"},
            {"KP","Nordkorea"},
            {"KR","Südkorea"},
            {"KW","Kuwait"},
            {"KY","Kaimaninseln"},
            {"KZ","Kasachstan"},
            {"LA","Laos"},
            {"LB","Libanon"},
            {"LC","St. Lucia"},
            {"LI","Liechtenstein"},
            {"LK","Sri Lanka"},
            {"LR","Liberia"},
            {"LS","Lesotho"},
            {"LT","Litauen"},
            {"LU","Luxemburg"},
            {"LV","Lettland"},
            {"LY","Libyen"},
            {"MA","Marokko"},
            {"MC","Monaco"},
            {"MD","Moldawien"},
            {"ME","Montenegro"},
            {"MF","St. Martin"},
            {"MG","Madagaskar"},
            {"MH","Marshallinseln"},
            {"MK","Nordmazedonien"},
            {"ML","Mali"},
            {"MM","Myanmar"},
            {"MN","Mongolei"},
            {"MO","Macao"},
            {"MP","Nördliche Marianen"},
            {"MQ","Martinique"},
            {"MR","Mauretanien"},
            {"MS","Montserrat"},
            {"MT","Malta"},
            {"MU","Mauritius"},
            {"MV","Malediven"},
            {"MW","Malawi"},
            {"MX","Mexiko"},
            {"MY","Malaysia"},
            {"MZ","Mosambik"},
            {"NA","Namibia"},
            {"NC","Neukaledonien"},
            {"NE","Niger"},
            {"NF","Norfolkinsel"},
            {"NG","Nigeria"},
            {"NI","Nicaragua"},
            {"NL","Niederlande"},
            {"NO","Norwegen"},
            {"NP","Nepal"},
            {"NR","Nauru"},
            {"NU","Niue"},
            {"NZ","Neuseeland"},
            {"OM","Oman"},
            {"PA","Panama"},
            {"PE","Peru"},
            {"PF","Französisch-Polynesien"},
            {"PG","Papua-Neuguinea"},
            {"PH","Philippinen"},
            {"PK","Pakistan"},
            {"PL","Polen"},
            {"PM","St. Pierre und Miquelon"},
            {"PN","Pitcairninseln"},
            {"PR","Puerto Rico"},
            {"PS","Palästina"},
            {"PT","Portugal"},
            {"PW","Palau"},
            {"PY","Paraguay"},
            {"QA","Katar"},
            {"RE","Réunion"},
            {"RO","Rumänien"},
            {"RS","Serbien"},
            {"RU","Russland"},
            {"RW","Ruanda"},
            {"SA","Saudi-Arabien"},
            {"SB","Salomonen"},
            {"SC","Seychellen"},
            {"SD","Sudan"},
            {"SE","Schweden"},
            {"SG","Singapur"},
            {"SH","St. Helena"},
            {"SI","Slowenien"},
            {"SJ","Svalbard und Jan Mayen"},
            {"SK","Slowakei"},
            {"SL","Sierra Leone"},
            {"SM","San Marino"},
            {"SN","Senegal"},
            {"SO","Somalia"},
            {"SR","Suriname"},
            {"SS","Südsudan"},
            {"ST","São Tomé und Príncipe"},
            {"SV","El Salvador"},
            {"SX","Sint Maarten"},
            {"SY","Syrien"},
            {"SZ","Eswatini"},
            {"TC","Turks- und Caicosinseln"},
            {"TD","Tschad"},
            {"TF","Französische Süd- und Antarktisgebiete"},
            {"TG","Togo"},
            {"TH","Thailand"},
            {"TJ","Tadschikistan"},
            {"TK","Tokelau"},
            {"TL","Osttimor"},
            {"TM","Turkmenistan"},
            {"TN","Tunesien"},
            {"TO","Tonga"},
            {"TR","Türkei"},
            {"TT","Trinidad und Tobago"},
            {"TV","Tuvalu"},
            {"TW","Taiwan"},
            {"TZ","Tansania"},
            {"UA","Ukraine"},
            {"UG","Uganda"},
            {"UM","Amerikanisch-Ozeanien"},
            {"US","Vereinigte Staaten"},
            {"UY","Uruguay"},
            {"UZ","Usbekistan"},
            {"VA","Vatikanstadt"},
            {"VC","St. Vincent und die Grenadinen"},
            {"VE","Venezuela"},
            {"VG","Britische Jungferninseln"},
            {"VI","Amerikanische Jungferninseln"},
            {"VN","Vietnam"},
            {"VU","Vanuatu"},
            {"WF","Wallis und Futuna"},
            {"WS","Samoa"},
            {"YE","Jemen"},
            {"YT","Mayotte"},
            {"ZA","Südafrika"},
            {"ZM","Sambia"},
            {"ZW","Simbabwe"}
        };

        Dictionary<string, string> typeTranslations = new Dictionary<string, string>()
        {
            {"small_airport","Kleiner Flughafen"},
            {"medium_airport","Mittlerer Flughafen"},
            {"large_airport","Großer Flughafen"},
            {"seaplane_base","Wasserflugzeugbasis"},
            {"heliport","Hubschrauberlandeplatz"},
            {"closed","Geschlossen"},
            {"balloonport","Ballonlandeplatz"}
        };

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
                if (lastModified < DateTime.Now.AddDays(-7))
                {
                    MessageBoxResult result = MessageBox.Show("Die Datei 'airports.csv' ist älter als 7 Tage. Möchten Sie sie erneut herunterladen?", "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);

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

            var continents = airports.Select(a => a.continent).Distinct().ToList();
            continents.Sort();
            foreach (var continent in continents)
            {
                string translatedContinent = continentTranslations.ContainsKey(continent) ? continentTranslations[continent] : continent;
                cbContinent.Items.Add(translatedContinent);
            }
        }

        private void cbContinent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbContinent.SelectedItem != null)
            {
                var keyOfSelectedContinent = continentTranslations
                    .FirstOrDefault(x => x.Value == cbContinent.SelectedItem.ToString()).Key;
                cbCountry.IsEnabled = true;
                cbCountry.Items.Clear();
                var countries = airports.Where(a => a.continent == keyOfSelectedContinent).Select(a => a.iso_country).Distinct().ToList();
                countries.Sort();
                foreach (var country in countries)
                {
                    string translatedCountry = countryTranlations.ContainsKey(country) ? countryTranlations[country] : country;
                    cbCountry.Items.Add(translatedCountry);
                }
            }
        }

        private void cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountry.SelectedItem != null)
            {
                var keyOfSelectedCountry = countryTranlations
                    .FirstOrDefault(x => x.Value == cbCountry.SelectedItem.ToString()).Key;
                cbType.IsEnabled = true;
                cbType.Items.Clear();
                var types = airports.Where(a => a.iso_country == keyOfSelectedCountry).Select(a => a.type).Distinct().ToList();
                types.Sort();
                foreach (var type in types)
                {
                    string translatedType = typeTranslations.ContainsKey(type) ? typeTranslations[type] : type;
                    cbType.Items.Add(translatedType);
                }
            }
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedItem != null)
            {
                var keyOfSelectedCountry = countryTranlations
                    .FirstOrDefault(x => x.Value == cbCountry.SelectedItem.ToString()).Key;
                var keyOfSelectedType = typeTranslations
                    .FirstOrDefault(x => x.Value == cbType.SelectedItem.ToString()).Key;
                cbAirport.IsEnabled = true;
                cbAirport.Items.Clear();
                var airportsFiltered = airports.Where(a => a.iso_country == keyOfSelectedCountry && a.type == keyOfSelectedType).Select(a => a.name).Distinct().ToList();
                airportsFiltered.Sort();
                foreach (var airport in airportsFiltered)
                {
                    
                    cbAirport.Items.Add(airport);
                }
            }
            lblFooter.Content = $"Anzahl der Flughäfen: {cbAirport.Items.Count}";
        }

        private void cbAirport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             if (cbAirport.SelectedItem != null)
            {
                var keyOfSelectedContinent = continentTranslations
                    .FirstOrDefault(x => x.Value == cbContinent.SelectedItem.ToString()).Key;
                var keyOfSelectedCountry = countryTranlations
                .FirstOrDefault(x => x.Value == cbCountry.SelectedItem.ToString()).Key;
                var keyOfSelectedType = typeTranslations
                    .FirstOrDefault(x => x.Value == cbType.SelectedItem.ToString()).Key;
                 var keyOfSelectedAirport = cbAirport.SelectedItem.ToString();
                 var airportFiltered = airports.Where(a => a.iso_country == keyOfSelectedCountry && a.type == keyOfSelectedType && a.continent  == keyOfSelectedContinent && a.name == keyOfSelectedAirport).Select(a => a).ToList();

                foreach (var airport in airportFiltered)
                {
                        wv2Airport.Source = new Uri($"https://www.openstreetmap.org/#map=13/{airport.latitude_deg}/{airport.longitude_deg}");
                }
            }
              
             if (cbAirport.SelectedItem != null)
            {
                var keyOfSelectedContinent = continentTranslations
                    .FirstOrDefault(x => x.Value == cbContinent.SelectedItem.ToString()).Key;
                var keyOfSelectedCountry = countryTranlations
                .FirstOrDefault(x => x.Value == cbCountry.SelectedItem.ToString()).Key;
                var keyOfSelectedType = typeTranslations
                    .FirstOrDefault(x => x.Value == cbType.SelectedItem.ToString()).Key;
                 var keyOfSelectedAirport = cbAirport.SelectedItem.ToString();
                 var airportFiltered = airports.Where(a => a.iso_country == keyOfSelectedCountry && a.type == keyOfSelectedType && a.continent  == keyOfSelectedContinent && a.name == keyOfSelectedAirport).Select(a => a).ToList();

                foreach (var airport in airportFiltered)
                {
                    lblName.Content = airport.name;
                    lblGemeinde.Content = airport.municipality;
                    lblTyp.Content = typeTranslations.ContainsKey(airport.type) ? typeTranslations[airport.type] : airport.type;
                    lblBreitengrad.Content = airport.latitude_deg;
                    lblLängengrad.Content = airport.longitude_deg;
                    lblHöheÜberNull.Content = airport.elevation_ft * 0.3 +" m";
                }
            }
        }

        private void cbxDefault_Checked(object sender, RoutedEventArgs e)
        {
            //Programm zurücksetzen
            cbContinent.SelectedIndex = -1;
            cbCountry.SelectedIndex = -1;
            cbType.SelectedIndex = -1;
            cbAirport.SelectedIndex = -1;
            cbCountry.IsEnabled = false;
            cbType.IsEnabled = false;
            cbAirport.IsEnabled = false;
            wv2Airport.Source = new Uri($"https://ourairports.com/");
            lblName.Content = "";
            lblGemeinde.Content = "";
            lblTyp.Content = "";
            lblBreitengrad.Content = "";
            lblLängengrad.Content = "";
            lblHöheÜberNull.Content = "";
        }

        private void lblFooter_Loaded(object sender, RoutedEventArgs e)
        {
            lblFooter.Content = $"Anzahl der Flughäfen: {cbAirport.Items.Count}";
        }
    }
}