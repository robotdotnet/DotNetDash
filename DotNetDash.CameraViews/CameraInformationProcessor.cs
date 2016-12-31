using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NetworkTables.Tables;

namespace DotNetDash.CameraViews
{
    class CameraInformationProcessor : TableProcessor
    {
        public CameraInformationProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories) : base(name, table, processorFactories)
        {
        }

        public override string Name => "Camera Information View";

        protected override FrameworkElement GetViewCore() => new CameraInformationView();
    }

    [DashboardType(typeof(ITableProcessorFactory), "CameraInformation")]
    public sealed class CameraInformationProcessorFactory : ITableProcessorFactory
    {
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        [ImportingConstructor]
        public CameraInformationProcessorFactory([ImportMany] IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.processorFactories = processorFactories;
        }

        public TableProcessor Create(string subTable, ITable table)
        {
            return new CameraInformationProcessor(subTable, table, processorFactories);
        }
    }
}
