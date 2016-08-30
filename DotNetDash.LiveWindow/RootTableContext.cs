using NetworkTables.Tables;

namespace DotNetDash.LiveWindow
{
    public class RootTableContext : NetworkTableContext
    {
        private ITable statusTable;
        public RootTableContext(string tableName, ITable table) : base(tableName, table)
        {
            statusTable = table.GetSubTable("~STATUS~");
            statusTable.AddTableListener("LW Enabled", (changedTable, _, value, flags) =>
                Enabled = value.GetBoolean(), true);
        }

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; NotifyPropertyChanged(); }
        }

    }
}
