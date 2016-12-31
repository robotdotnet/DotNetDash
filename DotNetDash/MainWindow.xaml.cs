using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private INetworkTablesInterface ntInterface;
        public MainWindow()
        {
            InitializeComponent();

            ntInterface = GetService<INetworkTablesInterface>();
            ntInterface.OnConnectionChanged += (obj, args) => ConnectionIndicator.Fill = args.Connected ? Brushes.Green : Brushes.Red;
            ntInterface.OnDisconnect += (obj, args) => ConnectionIndicator.Fill = Brushes.Red;

            LoadCustomControls();
            if(new RoboRioConnectionWindow(ntInterface).ShowDialog() != true)
            {
                Close();
            }
            else
            {
                InitializeDashboard();
            }
        }

        private void LoadCustomControls()
        {
            var factories = (Application.Current as App).Container.GetExports<IViewProcessorFactory, ICustomViewFactoryMetadata>();
            foreach (var factory in factories)
            {
                InsertMenu.Items.Add(new MenuItem {
                    Header = factory.Metadata.Name,
                    Command = new Command(() => AddViewToCurrentRootView(factory.Metadata.Name, factory.Value.Create()))
                });
            }
        }

        private void AddViewToCurrentRootView(string name, IViewProcessor viewProcessor)
        {
            var currentRootProcessor = (TableProcessor)Tabs.SelectedContent;
            currentRootProcessor.AddViewProcessorToView($"{name}_{Guid.NewGuid()}", viewProcessor);
        }

        protected override void OnClosed(EventArgs e)
        {
            ntInterface.Disconnect();
            base.OnClosed(e);
        }

        private void OpenRoboRioConnectionWindow(object sender, RoutedEventArgs e)
        {
            new RoboRioConnectionWindow(ntInterface).ShowDialog();
            InitializeDashboard();
        }

        private void InitializeDashboard()
        {
            InitializeTabs();
        }

        private void InitializeTabs()
        {
            Tabs.Items.Clear();
            var rootViews = (Application.Current as App).Container.GetExports<IRootTableProcessorFactory, IDashboardTypeMetadata>();
            foreach (var rootTable in Properties.Settings.Default.RootTables)
            {
                Tabs.Items.Add(new TabItem
                {
                    Content = CreateRootTableProcessor(rootViews, rootTable),
                    Header = rootTable
                });
            }
        }

        private void OpenServerConnectionWindow(object sender, RoutedEventArgs e)
        {
            new ServerConnectionWindow(ntInterface).ShowDialog();
            InitializeDashboard();
        }

        private TableProcessor CreateRootTableProcessor(IEnumerable<Lazy<IRootTableProcessorFactory, IDashboardTypeMetadata>> factories, string tableName)
        {
            var matchedProcessors = factories.Where(factory => factory.Metadata.IsMatch(tableName));
            var processor = (matchedProcessors.FirstOrDefault(factory => !factory.Metadata.IsWildCard()) ?? matchedProcessors.First())
                                  .Value.Create(tableName, ntInterface.GetTable(tableName));
            return processor;
        }

        private static T GetService<T>()
        {
            var app = App.Current as App;
            return app.Container.GetExport<T>().Value;
        }
    }
}
