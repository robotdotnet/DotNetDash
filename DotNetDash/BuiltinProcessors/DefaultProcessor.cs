using NetworkTables.Tables;
using System.ComponentModel.Composition.Hosting;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using NetworkTables;
using System.Threading;

namespace DotNetDash.BuiltinProcessors
{
    class DefaultProcessor : TableProcessor
    {
        public DefaultProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            :base(name, table, processorFactories)
        {
            baseTable.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current, (sendingTable, key, value, flags) =>
            {
                if (key == "~TYPE~") return;
                var stackPanel = (StackPanel)View;
                stackPanel.Children.Add(CreateNewElementView(key, value));
            }, NotifyFlags.NotifyImmediate | NotifyFlags.NotifyNew);
        }

        private UIElement CreateNewElementView(string key, object value)
        {
            var keyValueLine = new StackPanel { Orientation = Orientation.Horizontal };
            keyValueLine.Children.Add(new Label { Content = key });
            var valueBox = new TextBox();
            valueBox.SetBinding(TextBox.TextProperty, $"[{key}]");
            keyValueLine.Children.Add(valueBox);
            return keyValueLine;
        }

        protected override FrameworkElement GetViewCore()
        {
            return new StackPanel { Orientation = Orientation.Vertical };
        }
    }
}