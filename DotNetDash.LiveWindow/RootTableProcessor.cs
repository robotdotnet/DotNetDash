using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NetworkTables.Tables;
using System.Windows.Media;
using System.Windows.Data;
using System.Globalization;
using DotNetDash.BuiltinProcessors;

namespace DotNetDash.LiveWindow
{
    class RootTableProcessor : DefaultRootTableProcessor
    {
        public RootTableProcessor(string name, ITable table, CompositionContainer container) : base(name, table, container)
        {
        }

        protected override Panel GetPanelLayout()
        {
            return (Panel)element.Value.FindName("LiveWindowView");
        }

        protected override FrameworkElement GetViewCore()
        {
            var tab = (TabItem)base.GetViewCore();
            var enabledBody = (FrameworkElement)tab.Content;
            tab.Content = null;
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
            enabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new LWEnabledToVisibilityConverter() });
            disabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new LWDisabledToVisibilityConverter() });
            outline.Children.Add(enabledBody);
            outline.Children.Add(disabledBody);
            tab.Content = outline;
            return tab;
        }

        protected override NetworkTableContext GetTableContext(string name, ITable table) => new RootTableContext(name, table);

        private class LWEnabledToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value as bool?) == true ? Visibility.Visible : Visibility.Hidden;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        private class LWDisabledToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value as bool?) == true ? Visibility.Hidden : Visibility.Visible;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }

    [DashboardType(typeof(IRootTableProcessorFactory), nameof(LiveWindow))]
    public sealed class RootTableProcessorFactory : IRootTableProcessorFactory
    {
        public TableProcessor Create(string subTable, ITable table, CompositionContainer container)
        {
            return new RootTableProcessor(subTable, table, container);
        }
    }
}
