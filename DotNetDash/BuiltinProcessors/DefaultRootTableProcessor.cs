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
using System.Collections.ObjectModel;

namespace DotNetDash.BuiltinProcessors
{
    public class DefaultRootTableProcessor : TableProcessor
    {
        public DefaultRootTableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
            SubTableProcessorMap.Add(new ComparableTable(name, table),
                new ObservableCollection<TableProcessor>
                {
                    new DefaultProcessor(name, table, processorFactories)
                });
        }

        protected override FrameworkElement GetViewCore()
        {
            var tab = new TabItem();
            tab.SetBinding(HeaderedContentControl.HeaderProperty, nameof(NetworkTableContext.Name));
            var content = CreateSubTableHolder("RootTableStyle");
            tab.Content = content;
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
