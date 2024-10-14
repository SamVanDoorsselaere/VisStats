using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsBL.Model;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_Statistieken
{
    public partial class MainWindow : Window
    {
        string conn = "Data Source=LAPTOP-6EGCK7EE\\SQLEXPRESS;Initial Catalog=VisStats1F;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private IFileProcessor fileProcessor;
        private IVisStatsRepository visStatsRepository;
        private VisStatsManager visStatsManager;
        private ObservableCollection<Vissoort> AlleVissoorten;
        private ObservableCollection<Vissoort> GeselecteerdeVissoorten;

        public MainWindow()
        {
            InitializeComponent();

            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);
            HavensComboBox.ItemsSource = visStatsManager.GeefHavens();
            HavensComboBox.SelectedIndex = 0;
            JaarComboBox.ItemsSource = visStatsManager.GeefJaartallen();
            JaarComboBox.SelectedIndex = 0;
            AlleVissoorten = new ObservableCollection<Vissoort>(visStatsManager.GeefVissoorten());
            AlleSoortenListBox.ItemsSource = AlleVissoorten;
            GeselecteerdeVissoorten = new ObservableCollection<Vissoort>();
            GeselecteerdeSoortenListBox.ItemsSource = GeselecteerdeVissoorten;
        }

        private void VoegAlleSoortenToeButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Vissoort v in AlleVissoorten)
            {
                GeselecteerdeVissoorten.Add(v);
            }
            AlleVissoorten.Clear();
        }

        private void VoegSoortenToeButton_Click(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in AlleSoortenListBox.SelectedItems) soorten.Add(v);
            foreach (Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Add(v);
                AlleVissoorten.Remove(v);
            }
        }

        private void VerwijderSoortenButton_Click(object sender, RoutedEventArgs e)
        {
            List<Vissoort> soorten = new();
            foreach (Vissoort v in GeselecteerdeSoortenListBox.SelectedItems) soorten.Add(v);
            foreach (Vissoort v in soorten)
            {
                GeselecteerdeVissoorten.Add(v);
                AlleVissoorten.Remove(v);
            }
        }

        private void VerwijderAlleSoortenButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Vissoort v in AlleVissoorten)
            {
                GeselecteerdeVissoorten.Add(v);
            }
            GeselecteerdeVissoorten.Clear();
        }

        private void ToonStatistiekenButton_Click(object sender, RoutedEventArgs e)
        {
            Eenheid eenheid;
            if ((bool)KgRadioButton.IsChecked) eenheid = Eenheid.kg; else eenheid = Eenheid.euro;
            List<JaarVangst> vangst = visStatsManager.GeefVangst((int)JaarComboBox.SelectedItem, 
                (Haven)HavensComboBox.SelectedItem, GeselecteerdeVissoorten.ToList(), eenheid);
            StatistiekenWindow w = new StatistiekenWindow((int)JaarComboBox.SelectedItem,
                               (Haven)HavensComboBox.SelectedItem, vangst, eenheid);
            w.ShowDialog();
        }
    }
}