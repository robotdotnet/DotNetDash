using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using NetworkTables.Tables;

namespace DotNetDash.SpeedController
{
    class PWMControllerProcessor : TableProcessor
    {
        public PWMControllerProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override FrameworkElement GetViewCore()
        {
            return new SpeedControllerView();
        }

        protected override NetworkTableContext GetTableContext(string name, ITable table) => new ControllerModel(name, table);

        public override string Name => "PWM Speed Controller View";
    }

    [DashboardType(typeof(ITableProcessorFactory), "Speed Controller")]
    public sealed class PWMControllerProcessorFactory : ITableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public PWMControllerProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new PWMControllerProcessor(subTable, table, processorFactories);
        }
    }
}
