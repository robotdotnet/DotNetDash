using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using FRC.NetworkTables;

namespace DotNetDash.SpeedController
{
    class CANControllerProcessor : TableProcessor
    {
        public CANControllerProcessor(string name, NetworkTable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override FrameworkElement GetViewCore()
        {
            return new CANView();
        }

        protected override NetworkTableContext GetTableContext(string name, NetworkTable table) => new ControllerModel(name, table);

        public override string Name => "CAN Speed Controller View";
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

        public TableProcessor Create(string subTable, NetworkTable table)
        {
            return new CANControllerProcessor(subTable, table, processorFactories);
        }
    }
}
