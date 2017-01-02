using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using Serilog;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DotNetDash.Properties.Settings.Default.Save();
            Container.GetExportedValue<INetworkTablesInterface>().Disconnect();
        }

        private static ComposablePartCatalog CreateExtensionCatalog()
        {
            if (!Directory.Exists("Plugins"))
                return new AssemblyCatalog(typeof(App).Assembly);
            var extensionRootDirectory = new DirectoryInfo("Plugins");
            var catalog = new AggregateCatalog(from directory in extensionRootDirectory.EnumerateDirectories()
                                               select new DirectoryCatalog(directory.FullName));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));
            return catalog;
        }

        private CompositionContainer Container { get; set; }

        [Import]
        public IConnectionPrompts ConnectionPrompts { get; set; }

        [Import]
        public INetworkTablesInterface NetworkTables { get; set; }

        [Import]
        public MainWindow Dashboard { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Container = new CompositionContainer(CreateExtensionCatalog());
            Directory.CreateDirectory("./logs");
            Log.Logger = new LoggerConfiguration().ReadFrom.AppSettings().Enrich.WithProperty("Table", "Application Core").CreateLogger();

            Container.SatisfyImportsOnce(this);
            
            var teamNumber = ConnectionPrompts.PromptTeamNumber();
            if (teamNumber == null)
            {
                Shutdown();
                return;
            }
            NetworkTables.Disconnect();
            NetworkTables.ConnectToTeam(teamNumber.Value);
            MainWindow = Dashboard;
            MainWindow.Show();
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}
