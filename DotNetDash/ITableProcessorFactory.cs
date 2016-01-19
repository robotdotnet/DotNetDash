using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition.Hosting;

namespace DotNetDash
{
    public interface ITableProcessorFactory
    {
        TableProcessor Create(string subTable, ITable table, CompositionContainer container);
    }
}
