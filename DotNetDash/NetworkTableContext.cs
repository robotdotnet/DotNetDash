﻿using NetworkTables.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DotNetDash
{
    public class NetworkTableContext : INotifyPropertyChanged
    {
        private ITable table;

        public NetworkTableContext(string tableName, ITable table)
        {
            Name = tableName;
            this.table = table;
            table.AddTableListenerOnSynchronizationContext(SynchronizationContext.Current,
                (changedTable, key, value, flags) => NotifyPropertyChanged(System.Windows.Data.Binding.IndexerName));
        }

        public string Name { get; }

        public object this[string key]
        {
            get
            {
                return table.GetValue(key, null);
            }
            set
            {
                table.PutValue(key, value);
                NotifyPropertyChanged(System.Windows.Data.Binding.IndexerName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}