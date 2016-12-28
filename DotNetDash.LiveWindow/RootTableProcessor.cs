﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using DotNetDash.BuiltinProcessors;
using NetworkTables.Tables;

namespace DotNetDash.LiveWindow
{
    class RootTableProcessor : DefaultRootTableProcessor
    {
        public RootTableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
            baseTable.AddSubTableListenerOnSynchronizationContext(SynchronizationContext.Current, (tbl, subTableName, flags) =>
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
            enabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new LWEnabledToVisibilityConverter() });
            disabledBody.SetBinding(UIElement.VisibilityProperty, new Binding("Enabled") { Converter = new LWDisabledToVisibilityConverter() });
            outline.Children.Add(enabledBody);
            outline.Children.Add(disabledBody);
            return outline;
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
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public RootTableProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new RootTableProcessor(subTable, table, processorFactories);
        }
    }
}
