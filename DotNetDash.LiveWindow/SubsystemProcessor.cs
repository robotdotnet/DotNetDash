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
using System.ComponentModel.Composition;

namespace DotNetDash.LiveWindow
{
    class SubsystemProcessor : TableProcessor
    {
        public SubsystemProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
            : base(name, table, processorFactories)
        {
        }

        protected override FrameworkElement GetViewCore()
        {
            return CreateSubTableHolder("SubTableContainerStyle");
        }
    }

    [DashboardType(typeof(ITableProcessorFactory), "LW Subsystem")]
    public sealed class SubsystemProcessorFactory : ITableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public SubsystemProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new SubsystemProcessor(subTable, table, processorFactories);
        }
    }
}
