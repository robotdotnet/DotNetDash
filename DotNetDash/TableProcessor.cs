using Hellosam.Net.Collections;
using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace DotNetDash
{
    public abstract class TableProcessor : INotifyPropertyChanged
    {
        protected readonly ITable baseTable;
        protected readonly string name;
        protected ObservableDictionary<ComparableTable, ObservableCollection<TableProcessor>> subTableToProcessorsMap = new ObservableDictionary<ComparableTable, ObservableCollection<TableProcessor>>();

        public ObservableDictionary<ComparableTable, ObservableCollection<TableProcessor>> SubTableProcessorMap
        {
            get { return subTableToProcessorsMap; }
            set { subTableToProcessorsMap = value; NotifyPropertyChanged(); }
        }
        
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        public event PropertyChangedEventHandler PropertyChanged;

        protected TableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.name = name;
            baseTable = table;
            this.processorFactories = processorFactories;
            InitCurrentSubTables();
            InitProcessorListener();
        }

        private FrameworkElement view;

        public FrameworkElement View
        {
            get
            {
                if (view != null)
                {
                    return view;
                }
                else
                {
                    return view = GetBoundView();
                }
            }
            private set
            {
                view = value;
                TryBindView(value);
                NotifyPropertyChanged();
            }
        }

        private FrameworkElement GetBoundView()
        {
            var view = GetViewCore();
            TryBindView(view);
            return view;
        }

        private void TryBindView(FrameworkElement view)
        {
            if (view != null)
            {

                view.DataContext = GetTableContext(name, baseTable); 
            }
        }

        protected virtual NetworkTableContext GetTableContext(string name, ITable table) => new NetworkTableContext(name, table);

        protected abstract FrameworkElement GetViewCore();
        
        private void InitCurrentSubTables()
        {
            foreach (var subTableName in baseTable.GetSubTables())
            {
                AddProcessorOptionsForTable(subTableName);
            }
        }

        private void InitProcessorListener()
        {
            baseTable.AddSubTableListenerOnSynchronizationContext(SynchronizationContext.Current,
                (table, newTableName, flags) => AddProcessorOptionsForTable(newTableName));
        }

        private void AddProcessorOptionsForTable(string subTableName)
        {
            var subTable = baseTable.GetSubTable(subTableName);
            var tableType = subTable.GetString("~TYPE~", "");
            var selectedProcessors = new ObservableCollection<TableProcessor>(GetSortedTableProcessorsForType(subTable, subTableName, tableType));
            if (!subTableToProcessorsMap.ContainsKey(new ComparableTable(subTableName, subTable)))
            {
                subTableToProcessorsMap.Add(new ComparableTable(subTableName, subTable), selectedProcessors); 
            }
        }

        private IEnumerable<TableProcessor> GetSortedTableProcessorsForType(ITable table, string tableName, string tableType)
        {
            var matchedProcessorFactories = processorFactories.Where(factory => factory.Metadata.IsMatch(tableType)).ToList();
            matchedProcessorFactories.Sort((factory1, factory2) =>
            {
                if (factory1.Metadata.IsWildCard())
                    return factory2.Metadata.IsWildCard() ? 0 : 1;
                else if (factory2.Metadata.IsWildCard())
                    return factory1.Metadata.IsWildCard() ? 0 : -1;
                return 0;
            });
            return matchedProcessorFactories.Select(factory => factory.Value.Create(tableName, table));
        }

        private void NotifyPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        protected FrameworkElement CreateSubTableHolder(string styleName)
        {
            var content = new ItemsControl
            {
                Style = (Style)Application.Current.Resources[styleName],
                DataContext = this
            };
            content.SetBinding(ItemsControl.ItemsSourceProperty, nameof(SubTableProcessorMap));
            return new ContentControl { Content = content };
        }
    }
}
