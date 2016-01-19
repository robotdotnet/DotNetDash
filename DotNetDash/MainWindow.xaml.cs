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
                InitializeTabs();
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
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            Tabs.Items.Clear();
            var rootViews = (App.Current as App).Container.GetExports<IRootTableProcessorFactory, IDashboardTypeMetadata>();
            Tabs.Items.Add(CreateRootTableProcessor(rootViews, "SmartDashboard").GetBoundView());
            Tabs.Items.Add(CreateRootTableProcessor(rootViews, "LiveWindow").GetBoundView());
        }

        private void OpenServerConnectionWindow(object sender, RoutedEventArgs e)
        {
            new ServerConnectionWindow().ShowDialog();
            InitializeTabs();
        }

        private static TableProcessor CreateRootTableProcessor(IEnumerable<Lazy<IRootTableProcessorFactory, IDashboardTypeMetadata>> factories, string tableName)
        {
            var matchedProcessors = factories.Where(factory => factory.Metadata.IsMatch(tableName));
            var processor = (matchedProcessors.FirstOrDefault(factory => !factory.Metadata.IsWildCard()) ?? factories.First())
                                  .Value.Create(tableName, NetworkTables.NetworkTable.GetTable(tableName), (App.Current as App).Container);
            return processor;
        }
    }
}
