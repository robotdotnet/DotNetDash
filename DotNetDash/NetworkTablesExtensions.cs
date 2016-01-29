using NetworkTables;
using NetworkTables.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DotNetDash
{
    public static class NetworkTablesExtensions
    {
        public static void AddSubTableListenerOnDispatcher(this ITable table, Dispatcher dispatcher, Action<ITable, string, NotifyFlags> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            table.AddSubTableListener((tbl, name, _, flags) =>
            {
                dispatcher.InvokeAsync(() => callback(tbl, name, flags));
            });
        }

        public static void AddTableListenerOnDispatcher(this ITable table, Dispatcher dispatcher, Action<ITable, string, object, NotifyFlags> callback, bool immediateNotify = false)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            table.AddTableListener((tbl, name, value, flags) =>
            {
                dispatcher.InvokeAsync(() => callback(tbl, name, value, flags));
            }, immediateNotify);
        }

        public static void AddTableListenerOnDispatcher(this ITable table, Dispatcher dispatcher, Action<ITable, string, object, NotifyFlags> callback, NotifyFlags flags)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            table.AddTableListenerEx((tbl, name, value, _flags) =>
            {
                dispatcher.InvokeAsync(() => callback(tbl, name, value, _flags));
            }, flags);
        }
    }
}
