using FRC.NetworkTables;
using System;
using System.Diagnostics;
using System.Threading;

namespace DotNetDash
{
    public static class NetworkTablesExtensions
    {
        public static void AddSubTableListenerOnSynchronizationContext(this NetworkTable table, SynchronizationContext context, Action<NetworkTable, string> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            table.AddSubTableListener((NetworkTable tbl, ReadOnlySpan<char> key, NotifyFlags flags) =>
            {
                var name = key.ToString();
                if (context != null)
                {
                    context.Post(state => callback(tbl, name), null);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(state => callback(tbl, name), null);
                }
            }, false);
        }

        public static void AddTableListenerOnSynchronizationContext(this NetworkTable table, SynchronizationContext context, Action<NetworkTable, string, NetworkTableValue, NotifyFlags> callback, bool immediateNotify = false)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            NotifyFlags flags = NotifyFlags.Update;
            if (immediateNotify) flags |= NotifyFlags.Immediate;
            table.AddEntryListener((NetworkTable tbl, ReadOnlySpan<char> key, in NetworkTableEntry entry, in RefNetworkTableValue value, NotifyFlags flgs) =>
            {
                var name = key.ToString();
                var v = value.ToValue();
                if (context != null)
                {
                    context.Post(state => callback(tbl, name, v, flgs), null);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(state => callback(tbl, name, v, flgs), null);
                }
            }, flags);
        }

        public static void AddTableListenerOnSynchronizationContext(this NetworkTable table, SynchronizationContext context, Action<NetworkTable, string, NetworkTableValue, NotifyFlags> callback, NotifyFlags flags)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            table.AddEntryListener((NetworkTable tbl, ReadOnlySpan<char> key, in NetworkTableEntry entry, in RefNetworkTableValue value, NotifyFlags flgs) =>
            {
                var name = key.ToString();
                var v = value.ToValue();
                if (context != null)
                {
                    context.Post(state => callback(tbl, name, v, flgs), null);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(state => callback(tbl, name, v, flgs), null);
                }
            }, flags);
        }

        public static void AddGlobalConnectionListenerOnSynchronizationContext(SynchronizationContext context, Action<bool> callback, bool notifyImmediate)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            NetworkTableInstance.Default.AddConnectionListener((in ConnectionNotification notification) =>
            {
                var connected = notification.Connected;
                if (context != null)
                {
                    context.Post(state => callback(connected), null);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(state => callback(connected), null);
                }
            }, notifyImmediate);
        }
    }
}
