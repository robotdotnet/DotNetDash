using NetworkTables.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DotNetDash
{
    public class NetworkTableContext : INotifyPropertyChanged
    {
        private ITable table;

        public NetworkTableContext(string tableName, ITable table)
        {
            Name = tableName;
            this.table = table;
            Numbers = new NetworkTableBackedLookup<double>(table);
            Booleans = new NetworkTableBackedLookup<bool>(table);
            Strings = new NetworkTableBackedLookup<string>(table);
            Raw = new NetworkTableBackedLookup<byte[]>(table);
            StringArrays = new NetworkTableBackedLookup<string[]>(table);
            BooleanArrays = new NetworkTableBackedLookup<bool[]>(table);
            NumberArrays = new NetworkTableBackedLookup<double[]>(table);
        }

        public string Name { get; }

        public NetworkTableBackedLookup<double> Numbers { get; }

        public NetworkTableBackedLookup<bool> Booleans { get; }

        public NetworkTableBackedLookup<string> Strings { get; }

        public NetworkTableBackedLookup<byte[]> Raw { get; }

        public NetworkTableBackedLookup<string[]> StringArrays { get; }

        public NetworkTableBackedLookup<bool[]> BooleanArrays { get; }

        public NetworkTableBackedLookup<double[]> NumberArrays { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}