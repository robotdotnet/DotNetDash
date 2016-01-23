using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetworkTables.Tables;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel.Composition;

namespace DotNetDash.LiveWindow
{
    class SubsystemProcessor : TableProcessor
    {
        public SubsystemProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override Panel GetPanelLayout()
        {
            return (Panel)((Border)element.Value).Child;
        }

        protected override FrameworkElement GetViewCore()
        {
            var border = new Border { BorderBrush = Brushes.Black, BorderThickness = new Thickness(2) };
            var layout = new StackPanel { Orientation = Orientation.Vertical };
            layout.Children.Add(new Label { Content = name, HorizontalAlignment = HorizontalAlignment.Center});
            foreach (var processor in subTableProcessors)
            {
                layout.Children.Add(processor.GetBoundView());
            }
            border.Child = layout;
            return border;
        }
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
