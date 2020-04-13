using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Content = new WpfControls.DataVisualization.WpfNumericAxis();
        }
    }
}
