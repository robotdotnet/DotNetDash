using Hellosam.Net.Collections;
using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        protected ObservableCollection<TableProcessor> subTableProcessors = new ObservableCollection<TableProcessor>();
        private readonly IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories;

        public event PropertyChangedEventHandler PropertyChanged;

        protected TableProcessor(string name, ITable table, IEnumerable<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>> processorFactories)
        {
            this.name = name;
            baseTable = table;
            this.processorFactories = processorFactories;
            View = GetBoundView();
            InitProcessorListener();
        }

        private FrameworkElement view;

        public FrameworkElement View
        {
            get { return view; }
            private set { view = value; NotifyPropertyChanged(); }
        }

        private FrameworkElement GetBoundView()
        {
            var view = GetViewCore();
            view.DataContext = GetTableContext(name, baseTable);
            return view;
        }

        protected virtual NetworkTableContext GetTableContext(string name, ITable table) => new NetworkTableContext(name, table);

        protected abstract FrameworkElement GetViewCore();

        protected void InitProcessorListener()
        {
            baseTable.AddSubTableListenerOnDispatcher(Application.Current.Dispatcher, (table, newTableName, flags) => AddProcessorOptionsForTable(newTableName));
        }

        private void AddProcessorOptionsForTable(string newTableName)
        {
            var subTable = baseTable.GetSubTable(newTableName);
            var tableType = subTable.GetString("~TYPE~", "");
            var selectedProcessors = new ObservableCollection<TableProcessor>(GetSortedTableProcessorsForType(subTable, newTableName, tableType));
            subTableToProcessorsMap.Add(new ComparableTable(newTableName, subTable), selectedProcessors);
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


        protected ItemsControl CreateSubTableHolder(string styleName)
        {
            var content = new ItemsControl
            {
                Style = (Style)Application.Current.Resources[styleName],
                DataContext = this
            };
            content.SetBinding(ItemsControl.ItemsSourceProperty, nameof(SubTableProcessorMap));
            return content;
        }
    }
}
