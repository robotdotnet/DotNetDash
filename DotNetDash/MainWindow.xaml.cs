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

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if(new RoboRioConnectionWindow().ShowDialog() != true)
            {
                Close();
            }
            else
            {
                InitializeDashboard();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            NetworkTables.NetworkTable.Shutdown();
            base.OnClosed(e);
        }

        private void OpenRoboRioConnectionWindow(object sender, RoutedEventArgs e)
        {
            new RoboRioConnectionWindow().ShowDialog();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            InitializeTabs();
            InitializeConnectivityMarker();
        }

        private async void InitializeConnectivityMarker()
        {
            await Task.Delay(500); //Add a delay to the connection check so NetworkTables can establish the connection
            if(NetworkTables.NetworkTable.Connections().Any())
            {
                ConnectionIndicator.Fill = Brushes.Green;
            }
            else
            {
                ConnectionIndicator.Fill = Brushes.Red;
            }
        }

        private void InitializeTabs()
        {
            Tabs.Items.Clear();
            var rootViews = (Application.Current as App).Container.GetExports<IRootTableProcessorFactory, IDashboardTypeMetadata>();
            foreach (var rootTable in Properties.Settings.Default.RootTables)
            {
                Tabs.Items.Add(new TabItem
                {
                    Content = CreateRootTableProcessor(rootViews, rootTable).View,
                    Header = rootTable
                });
            }
        }

        private void OpenServerConnectionWindow(object sender, RoutedEventArgs e)
        {
            new ServerConnectionWindow().ShowDialog();
            InitializeDashboard();
        }

        private static TableProcessor CreateRootTableProcessor(IEnumerable<Lazy<IRootTableProcessorFactory, IDashboardTypeMetadata>> factories, string tableName)
        {
            var matchedProcessors = factories.Where(factory => factory.Metadata.IsMatch(tableName));
            var processor = (matchedProcessors.FirstOrDefault(factory => !factory.Metadata.IsWildCard()) ?? matchedProcessors.First())
                                  .Value.Create(tableName, NetworkTables.NetworkTable.GetTable(tableName));
            return processor;
        }
    }
}
