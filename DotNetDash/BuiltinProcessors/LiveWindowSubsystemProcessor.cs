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

namespace DotNetDash.BuiltinProcessors
{
    class LiveWindowSubsystemProcessor : TableProcessor
    {
        public LiveWindowSubsystemProcessor(string name, ITable table, CompositionContainer container) : base(name, table, container)
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

    [DashboardType(typeof(ITableProcessorFactory), "LW Subsystem", true)]
    public class LiveWindowSubsystemProcessorFactory : ITableProcessorFactory
    {
        public TableProcessor Create(string subTable, ITable table, CompositionContainer container)
        {
            return new LiveWindowSubsystemProcessor(subTable, table, container);
        }
    }
}
