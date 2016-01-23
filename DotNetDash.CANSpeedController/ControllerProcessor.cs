using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetworkTables.Tables;
using System.ComponentModel.Composition;

namespace DotNetDash.CANSpeedController
{
    class ControllerProcessor : TableProcessor
    {
        public ControllerProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override FrameworkElement GetViewCore()
        {
            return new CANView();
        }

        protected override NetworkTableContext GetTableContext(string name, ITable table) => new ControllerModel(name, table);
    }

    [DashboardType(typeof(ITableProcessorFactory), "CANSpeedController")]
    public sealed class ControllerProcessorFactory : ITableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public ControllerProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new ControllerProcessor(subTable, table, processorFactories);
        }
    }
}
