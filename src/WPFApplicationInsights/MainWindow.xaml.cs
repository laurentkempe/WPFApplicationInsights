using System.Threading.Tasks;
using System.Windows;
using Serilog;

namespace WPFApplicationInsights
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Log_OnClick(object sender, RoutedEventArgs e)
        {
            Log.Information("You entered {Text}", TextBox.Text);
        }

        private void TaskScheduler_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Run(() => throw new System.NotImplementedException("Task.Run exception"));
        }
    }
}
