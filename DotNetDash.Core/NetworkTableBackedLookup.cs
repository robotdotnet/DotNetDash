using FRC.NetworkTables;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DotNetDash
{
    public sealed class NetworkTableBackedLookup<T> : INotifyPropertyChanged
    {
        private readonly Type[] SupportedValueTypes =
        {
            typeof(string),
            typeof(double),
            typeof(byte[]),
            typeof(bool),
            typeof(string[]),
            typeof(double[]),
            typeof(bool[]),
        };

        private readonly NetworkTable table;

        public NetworkTableBackedLookup(NetworkTable table)
        {
            bool validType = SupportedValueTypes.Contains(typeof(T));
            
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
                var value = table.GetEntry(key).GetObjectValue();
                if (value == null) return default(T);
                try
                {
                    return (T)value;
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
            set
            {
                // TODO see if this needs a catch
                table.GetEntry(key).SetValue(value);
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
