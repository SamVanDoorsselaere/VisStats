using Microsoft.Win32;
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
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace VisStatsUI_DataUpload
{
    public partial class MainWindow : Window
    {
        OpenFileDialog dialog = new OpenFileDialog();
        string conn = "Data Source=LAPTOP-6EGCK7EE\\SQLEXPRESS;Initial Catalog=VisStats1F;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        IFileProcessor fileProcessor;
        IVisStatsRepository visStatsRepository;
        VisStatsManager visStatsManager;

        public MainWindow()
        {
            InitializeComponent();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.InitialDirectory = @"C:\data\vis\";
            dialog.Multiselect = true;
            fileProcessor = new FileProcessor();
            visStatsRepository = new VisStatsRepository(conn);
            visStatsManager = new VisStatsManager(fileProcessor, visStatsRepository);
        }

        private void Button_Click_Vissoorten(object sender, RoutedEventArgs e)
        {
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var filenames = dialog.FileNames;
                VissoortenFileListBox.ItemsSource = filenames;
                dialog.FileName = null;
            }
            else
            {
                VissoortenFileListBox.ItemsSource = null;
            }
        }

        private void Button_Click_UploadVissoorten(object sender, RoutedEventArgs e)
        {
            foreach(string filename in VissoortenFileListBox.ItemsSource)
            {
                visStatsManager.UploadVissoorten(filename);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }

        private void Button_Click_Havens(object sender, RoutedEventArgs e)
        {
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                var filenames = dialog.FileNames;
                HavensFileListBox.ItemsSource = filenames;
                dialog.FileName = null;
            }
            else
            {
                HavensFileListBox.ItemsSource = null;
            }
        }

        private void Button_Click_UploadHavens(object sender, RoutedEventArgs e)
        {
            foreach (string filename in HavensFileListBox.ItemsSource)
            {
                visStatsManager.UploadHavens(filename);
            }
            MessageBox.Show("Upload klaar", "VisStats");
        }
    }
}