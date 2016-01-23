using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetworkTables.Tables;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace DotNetDash.BuiltinProcessors
{
    public class DefaultRootTableProcessor : TableProcessor
    {
        public DefaultRootTableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override Panel GetPanelLayout()
        {
            return (Panel)((TabItem)element.Value).Content;
        }

        protected override FrameworkElement GetViewCore()
        {
            var tab = new TabItem();
            tab.SetBinding(HeaderedContentControl.HeaderProperty, nameof(NetworkTableContext.Name));
            var canvas = new Canvas();
            foreach (var processor in subTableProcessors)
            {
                canvas.Children.Add(processor.GetBoundView());
            }
            tab.Content = canvas;
            return tab;
        }
    }
    
    [DashboardType(typeof(IRootTableProcessorFactory), "")]
    public sealed class DefaultRootTableProcessorFactory : IRootTableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public DefaultRootTableProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new DefaultRootTableProcessor(subTable, table, processorFactories);
        }
    }
}
