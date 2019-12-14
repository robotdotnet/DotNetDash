using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DotNetDash.BuiltinProcessors;
using FRC.NetworkTables;

namespace DotNetDash.LiveWindow
{
    class RootTableProcessor : DefaultRootTableProcessor
    {
        public RootTableProcessor(string name, NetworkTable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
            baseTable.AddSubTableListenerOnSynchronizationContext(SynchronizationContext.Current, (tbl, subTableName) =>
            {
                if (subTableName == "~STATUS~")
                {
                    logger.Information("Removing ~STATUS~ table processors");
                    KeyToMultiProcessorMap.Remove("~STATUS~");
                }
            });
            logger.Information("Removing ~STATUS~ table processors");
            KeyToMultiProcessorMap.Remove("~STATUS~");
        }

        protected override FrameworkElement GetViewCore()
        {
            var enabledBody = base.GetViewCore();
            enabledBody.Name = "LiveWindowView";
            var disabledBody = new TextBlock
            {
                Text = "Run the robot in test mode to enable LiveWindow",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 15,
                Foreground = new SolidColorBrush(Colors.DarkRed)
            };
            var outline = new Grid();
            enabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new IsVisibleConverter() });
            disabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new IsHiddenConverter() });
            outline.Children.Add(enabledBody);
            outline.Children.Add(disabledBody);
            return outline;
        }

        protected override NetworkTableContext GetTableContext(string name, NetworkTable table) => new RootTableContext(name, table);

    }

    [DashboardType(typeof(IRootTableProcessorFactory), nameof(LiveWindow))]
    public sealed class RootTableProcessorFactory : IRootTableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public RootTableProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, NetworkTable table)
        {
            return new RootTableProcessor(subTable, table, processorFactories);
        }
    }
}
