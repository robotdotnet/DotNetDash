﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public MainWindow()
        {
            InitializeComponent();
            LoadCustomControls();
            if(new RoboRioConnectionWindow().ShowDialog() != true)
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

        private bool connectivityMarkerInitialized = false;

        private void InitializeConnectivityMarker()
        {
            if (!connectivityMarkerInitialized)
            {
                NetworkTables.NetworkTable.AddGlobalConnectionListener((remote, connection, connected) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        ConnectionIndicator.Fill = connected ? Brushes.Green : Brushes.Red;
                    });
                }, true);

                connectivityMarkerInitialized = true;
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
                    Content = CreateRootTableProcessor(rootViews, rootTable),
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
