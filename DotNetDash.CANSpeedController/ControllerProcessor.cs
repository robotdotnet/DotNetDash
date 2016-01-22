using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetworkTables.Tables;

namespace DotNetDash.CANSpeedController
{
    class ControllerProcessor : TableProcessor
    {
        public ControllerProcessor(string name, ITable table, CompositionContainer container) : base(name, table, container)
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
        public TableProcessor Create(string subTable, ITable table, CompositionContainer container)
        {
            return new ControllerProcessor(subTable, table, container);
        }
    }
}
