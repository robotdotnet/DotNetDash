using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Container = new CompositionContainer(CreateExtensionCatalog());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            DotNetDash.Properties.Settings.Default.Save();
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

        public CompositionContainer Container { get; private set; }
    }
}
