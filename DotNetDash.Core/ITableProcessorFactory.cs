using FRC.NetworkTables;

namespace DotNetDash
{
    public interface ITableProcessorFactory
    {
        TableProcessor Create(string subTable, NetworkTable table);
    }
}
