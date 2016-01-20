using DotNetDash.BuiltinProcessors;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        private static ComposablePartCatalog CreateExtensionCatalog()
        {
            var runtimeExportBuilder = new RegistrationBuilder();
            BuildRuntimeExports(runtimeExportBuilder);
            if (!Directory.Exists("Plugins"))
                return new AssemblyCatalog(typeof(App).Assembly, runtimeExportBuilder);
            var extensionRootDirectory = new DirectoryInfo("Plugins");
            var catalog = new AggregateCatalog(from directory in extensionRootDirectory.EnumerateDirectories()
                                               select new DirectoryCatalog(directory.FullName));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly, runtimeExportBuilder));
            return catalog;
        }

        private static void BuildRuntimeExports(RegistrationBuilder runtimeExportBuilder)
        {
            runtimeExportBuilder.ForType<XamlFileSearcher>().Export<IXamlSearcher>();
        }

        public CompositionContainer Container { get; private set; }
    }
}
