using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace DotNetDash.LiveWindow
{
    class SubsystemProcessor : TableProcessor
    {
        public SubsystemProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override FrameworkElement GetViewCore()
        {
            var panel = new StackPanel { Orientation = Orientation.Vertical };
            var nameBlock = new TextBlock { HorizontalAlignment = HorizontalAlignment.Center };
            nameBlock.SetBinding(TextBlock.TextProperty, nameof(NetworkTableContext.Name));
            panel.Children.Add(nameBlock);
            panel.Children.Add(CreateSubTableHolder("SubTableContainerStyle"));
            return panel;
        }

        public override string Name => "LiveWindow Subsystem View";
    }

    [DashboardType(typeof(ITableProcessorFactory), "LW Subsystem")]
    public sealed class SubsystemProcessorFactory : ITableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public SubsystemProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new SubsystemProcessor(subTable, table, processorFactories);
        }
    }
}
