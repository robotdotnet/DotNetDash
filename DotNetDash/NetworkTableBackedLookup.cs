using System;
using NetworkTables.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using NetworkTables;

namespace DotNetDash
{
    public sealed class NetworkTableBackedLookup<T> : INotifyPropertyChanged
    {
        private readonly ITable table;

        public NetworkTableBackedLookup(ITable table)
        {
            bool validType = false;
            foreach (var type in Value.GetSupportedValueTypes())
            {
                if (type == typeof(T))
                {
                    validType = true;
                    break;
                }
            }
            if (!validType)
            {
                throw new InvalidOperationException($"Generic type {typeof(T)} is not supported");
            }

            this.table = table;
            table.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current,
                (changedTable, key, value, flags) =>
                    NotifyPropertyChanged(System.Windows.Data.Binding.IndexerName));
        }

        public T this[string key]
        {
            get
            {
                var value = table.GetValue(key, null);
                if (value == null) return default(T);
                bool success;
                var rawVal = value.GetValue<T>(out success);
                return !success ? default(T) : rawVal;
            }
            set
            {
                var val = Value.MakeValue(value);
                if (val == null) return;
                table.PutValue(key, val);
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
