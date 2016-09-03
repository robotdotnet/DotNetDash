using NetworkTables.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DotNetDash
{
    public sealed class NetworkTableBackedLookup<T> : INotifyPropertyChanged
    {
        private readonly ITable table;

        public NetworkTableBackedLookup(ITable table)
        {
            this.table = table;
            table.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current,
                (changedTable, key, value, flags) =>
                    NotifyPropertyChanged(System.Windows.Data.Binding.IndexerName));
        }

        public T this[string key]
        {
            get
            {
                bool success = false;
                var value = table.GetValue(key, NetworkTables.Value.MakeValue(default(T)));
                return (value is T) ? value.GetValue<T>(out success) : default(T);
            }
            set
            {
                table.PutValue(key, NetworkTables.Value.MakeValue(value));
                NotifyPropertyChanged(System.Windows.Data.Binding.IndexerName);
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
