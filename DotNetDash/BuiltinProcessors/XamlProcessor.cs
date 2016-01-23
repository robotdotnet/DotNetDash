using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetDash.BuiltinProcessors
{
    class XamlProcessor : TableProcessor
    {
        private readonly XamlView view;

        public XamlProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories, XamlView view)
            :base(name, table, processorFactories)
        {
            this.view = view;
        }

        protected override FrameworkElement GetViewCore()
        {
            return view;
        }
    }
}
