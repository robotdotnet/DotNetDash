using FRC.NetworkTables;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DotNetDash.BuiltinProcessors
{
    class XamlProcessor : TableProcessor
    {
        private readonly XamlView view;

        public XamlProcessor(string name, NetworkTable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories, XamlView view)
            :base(name, table, processorFactories)
        {
            this.view = view;
        }

        protected override FrameworkElement GetViewCore()
        {
            return view;
        }

        public override string Name => "XAML-defined View";
    }
}
