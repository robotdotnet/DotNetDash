using FRC.NetworkTables;
using System;

namespace DotNetDash.LiveWindow
{
    public class RootTableContext : NetworkTableContext
    {
        private NetworkTable statusTable;
        public RootTableContext(string tableName, NetworkTable table) : base(tableName, table)
        {
            statusTable = table.GetSubTable("~STATUS~");
            statusTable.AddEntryListener("LW Enabled", (NetworkTable changedTable, ReadOnlySpan<char> _, in NetworkTableEntry entry, in RefNetworkTableValue value, NotifyFlags flags) =>
                Enabled = value.GetBoolean(), NotifyFlags.Update | NotifyFlags.Immediate);
        }

        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; NotifyPropertyChanged(); }
        }

    }
}
