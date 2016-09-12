using NetworkTables;
using NetworkTables.Tables;
using System;
using System.Collections.Generic;

namespace DotNetDash.Test
{
    class MockTable : ITable
    {
        private class ActionTableListener : ITableListener
        {
            private readonly Action<ITable, string, Value, NotifyFlags> listenerDelegate;

            public ActionTableListener(Action<ITable, string, Value, NotifyFlags> listenerDelegate)
            {
                this.listenerDelegate = listenerDelegate;
            }

            public void ValueChanged(ITable source, string key, Value value, NotifyFlags flags)
            {
                listenerDelegate(source, key, value, flags);
            }
        }

        private List<ITableListener> subtableListeners = new List<ITableListener>();
        private List<ITableListener> tableListeners = new List<ITableListener>();
        private Dictionary<string, dynamic> storage = new Dictionary<string, dynamic>();
        private Dictionary<string, ITable> subTables = new Dictionary<string, ITable>();



        private void RaiseListeners(List<ITableListener> listeners, string key, Value value, NotifyFlags flags)
        {
            foreach (var listener in listeners)
            {
                listener.ValueChanged(this, key, value, flags);
            }
        }


        public void AddSubTableListener(Action<ITable, string, Value, NotifyFlags> listenerDelegate)
        {
            subtableListeners.Add(new ActionTableListener(listenerDelegate));
        }

        public bool SetDefaultValue(string key, Value defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultNumber(string key, double defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultBoolean(string key, bool defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultString(string key, string defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultRaw(string key, byte[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultBooleanArray(string key, bool[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultNumberArray(string key, double[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public bool SetDefaultStringArray(string key, string[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public void AddSubTableListener(ITableListener listener)
        {
            subtableListeners.Add(listener);
        }

        public void AddSubTableListener(Action<ITable, string, Value, NotifyFlags> listenerDelegate, bool localNotify)
        {
            subtableListeners.Add(new ActionTableListener(listenerDelegate));
        }

        public void AddSubTableListener(ITableListener listener, bool localNotify)
        {
            subtableListeners.Add(listener);
        }

        public void AddTableListener(Action<ITable, string, Value, NotifyFlags> listenerDelegate, bool immediateNotify = false)
        {
            tableListeners.Add(new ActionTableListener(listenerDelegate));
        }

        public void AddTableListener(ITableListener listener, bool immediateNotify = false)
        {
            tableListeners.Add(listener);
        }

        public void AddTableListener(string key, Action<ITable, string, Value, NotifyFlags> listenerDelegate, bool immediateNotify)
        {
            throw new NotImplementedException();
        }

        public void AddTableListener(string key, ITableListener listener, bool immediateNotify)
        {
            throw new NotImplementedException();
        }

        public void AddTableListenerEx(Action<ITable, string, Value, NotifyFlags> listenerDelegate, NotifyFlags flags)
        {
            tableListeners.Add(new ActionTableListener(listenerDelegate));
        }

        public void AddTableListenerEx(ITableListener listener, NotifyFlags flags)
        {
            throw new NotImplementedException();
        }

        public void AddTableListenerEx(string key, Action<ITable, string, Value, NotifyFlags> listenerDelegate, NotifyFlags flags)
        {
            throw new NotImplementedException();
        }

        public void AddTableListenerEx(string key, ITableListener listener, NotifyFlags flags)
        {
            throw new NotImplementedException();
        }

        public void ClearFlags(string key, EntryFlags flags)
        {
            throw new NotImplementedException();
        }

        public void ClearPersistent(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsSubTable(string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(string key)
        {
            return storage[key];
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeBoolean(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public bool[] GetBooleanArray(string key)
        {
            return storage[key];
        }

        public bool[] GetBooleanArray(string key, bool[] defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeBooleanArray(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public EntryFlags GetFlags(string key)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> GetKeys()
        {
            return new HashSet<string>(storage.Keys);
        }

        public HashSet<string> GetKeys(NtType types)
        {
            throw new NotImplementedException();
        }

        public double GetNumber(string key)
        {
            return storage[key];
        }

        public double GetNumber(string key, double defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeDouble(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public double[] GetNumberArray(string key)
        {
            return storage[key];
        }

        public double[] GetNumberArray(string key, double[] defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeDoubleArray(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public byte[] GetRaw(string key)
        {
            throw new NotImplementedException();
        }

        public byte[] GetRaw(string key, byte[] defaultValue)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key)
        {
            return storage[key];
        }

        public string GetString(string key, string defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeString(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public string[] GetStringArray(string key)
        {
            return storage[key];
        }

        public string[] GetStringArray(string key, string[] defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, Value.MakeStringArray(defaultValue), NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public ITable GetSubTable(string key)
        {
            if(subTables.ContainsKey(key))
            {
                return subTables[key];
            }
            else
            {
                subTables[key] = new MockTable();
                RaiseListeners(subtableListeners, key, null, NotifyFlags.NotifyNew);
                return subTables[key];
            }
        }

        public HashSet<string> GetSubTables()
        {
            return new HashSet<string>(subTables.Keys);
        }

        public Value GetValue(string key)
        {
            return storage[key];
        }

        public Value GetValue(string key, Value defaultValue)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                storage[key] = defaultValue;
                RaiseListeners(tableListeners, key, defaultValue, NotifyFlags.NotifyNew);
                return storage[key];
            }
        }

        public bool IsPersistent(string key)
        {
            throw new NotImplementedException();
        }

        public bool PutBoolean(string key, bool value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeBoolean(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutBooleanArray(string key, bool[] value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeBooleanArray(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutNumber(string key, double value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeDouble(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutNumberArray(string key, double[] value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeDoubleArray(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutRaw(string key, byte[] value)
        {
            throw new NotImplementedException();
        }

        public bool PutString(string key, string value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeString(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutStringArray(string key, string[] value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, Value.MakeStringArray(value), NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public bool PutValue(string key, Value value)
        {
            storage[key] = value;
            RaiseListeners(tableListeners, key, value, NotifyFlags.NotifyNew & NotifyFlags.NotifyUpdate);
            return true;
        }

        public void RemoveTableListener(Action<ITable, string, Value, NotifyFlags> listenerDelegate)
        {
            throw new NotImplementedException();
        }

        public void RemoveTableListener(ITableListener listener)
        {
            throw new NotImplementedException();
        }

        public void SetFlags(string key, EntryFlags flags)
        {
            throw new NotImplementedException();
        }

        public void SetPersistent(string key)
        {
            throw new NotImplementedException();
        }
    }
}
