using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;

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
            return CreateSubTableHolder("RootTableStyle");
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
