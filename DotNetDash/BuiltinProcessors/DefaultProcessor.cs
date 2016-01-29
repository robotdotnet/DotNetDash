using NetworkTables;
using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DotNetDash.BuiltinProcessors
{
    class DefaultProcessor : TableProcessor
    {
        public DefaultProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
            baseTable.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current, (sendingTable, key, value, flags) =>
            {
                if (key == "~TYPE~") return;
                var stackPanel = (StackPanel)View;
                stackPanel.Children.Add(CreateNewElementView(key, value));
            }, NotifyFlags.NotifyImmediate | NotifyFlags.NotifyNew);
        }

        protected override FrameworkElement GetViewCore()
        {
            return new StackPanel { Orientation = Orientation.Vertical };
        }

        private UIElement CreateNewElementView(string key, object value)
        {
            var keyValueLine = new StackPanel { Orientation = Orientation.Horizontal };
            keyValueLine.Children.Add(new Label { Content = key });
            var valueBox = new TextBox();
            var typeCategory = DetermineValueNetworkType(value);
            valueBox.SetBinding(TextBox.TextProperty, $"{typeCategory}[{key}]");
            keyValueLine.Children.Add(valueBox);
            return keyValueLine;
        }

        private static string DetermineValueNetworkType(object value)
        {
            if (value is double)
            {
                return nameof(NetworkTableContext.Numbers);
            }
            else if (value is string)
            {
                return nameof(NetworkTableContext.Strings);
            }
            else if (value is bool)
            {
                return nameof(NetworkTableContext.Booleans);
            }
            else if (value is byte[])
            {
                return nameof(NetworkTableContext.Raw);
            }
            else if (value is double[])
            {
                return nameof(NetworkTableContext.NumberArrays);
            }
            else if (value is string[])
            {
                return nameof(NetworkTableContext.StringArrays);
            }
            else if (value is bool[])
            {
                return nameof(NetworkTableContext.BooleanArrays);
            }
            return string.Empty;
        }
    }
}