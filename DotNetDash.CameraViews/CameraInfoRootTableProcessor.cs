using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetDash.BuiltinProcessors;
using NetworkTables.Tables;

namespace DotNetDash.CameraViews
{
    class CameraInfoRootTableProcessor : DefaultRootTableProcessor
    {
        public CameraInfoRootTableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories) : base(name, table, processorFactories)
        {
        }

        protected override string DefaultTableType => "CameraInformation";
    }

    [DashboardType(typeof(IRootTableProcessorFactory), "CameraProcessor")]
    public sealed class CameraInfoRootTableProcessorFactory : IRootTableProcessorFactory
    {
        private IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public CameraInfoRootTableProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new CameraInfoRootTableProcessor(subTable, table, processorFactories);
        }
    }

}
