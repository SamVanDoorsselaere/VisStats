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
using System.Windows.Shapes;
using VisStatsBL.Model;

namespace VisStatsUI_Statistieken
{
    public partial class StatistiekenWindow : Window
    {
        public StatistiekenWindow(int jaar, Haven haven, List<JaarVangst> vangst, Eenheid eenheid)
        {
            InitializeComponent();
            HavenTextBox.Text = haven.ToString();
            JaarTextBox.Text = jaar.ToString();
            EenheidTextBox.Text = eenheid.ToString();
            StatistiekenDataGrid.AutoGeneratingColumn += StatistiekDataGrid_AutoGeneratingColumn;
            StatistiekenDataGrid.ItemsSource = vangst;
        }

        private void StatistiekDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(double) || e.PropertyType == typeof(float) || e.PropertyType == typeof(decimal))
            {
                var dataGridTextColumn = e.Column as DataGridTextColumn;
                if(dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "N2" ;
                    Style cellStyle = new Style(typeof(DataGridCell));
                    cellStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                    dataGridTextColumn.CellStyle = cellStyle;
                }
            }
        }
    }
}
