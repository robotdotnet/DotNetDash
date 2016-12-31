﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetDash;
using NetworkTables;
using NetworkTables.Tables;

namespace DotNetDash
{
    [Export(typeof(INetworkTablesInterface))]
    public class SingletonNetworkTablesInterface : INetworkTablesInterface
    {
        public event EventHandler<ConnectionChangedEventArgs> OnConnectionChanged;
        public event EventHandler OnDisconnect;

        public SingletonNetworkTablesInterface()
        {
            NetworkTablesExtensions.AddGlobalConnectionListenerOnSynchronizationContext(SynchronizationContext.Current,
                (remote, info, connected) => OnConnectionChanged?.Invoke(this, new ConnectionChangedEventArgs { Connected = connected }), true);
        }

        public void ConnectToServer(string server, int port)
        {
            NetworkTable.SetClientMode();
            NetworkTable.SetIPAddress(server);
            NetworkTable.SetPort(port);
            NetworkTable.Initialize();
        }

        public void ConnectToTeam(int team)
        {
            NetworkTable.SetClientMode();
            NetworkTable.SetTeam(team);
            NetworkTable.Initialize();
        }

        public ITable GetTable(string path)
        {
            return NetworkTable.GetTable(path);
        }

        public void Disconnect()
        {
            NetworkTable.Shutdown();
            OnDisconnect?.Invoke(this, EventArgs.Empty);
        }
    }
}
