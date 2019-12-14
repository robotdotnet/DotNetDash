using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetDash;
using FRC.NetworkTables;

namespace DotNetDash
{
    [Export(typeof(INetworkTablesInterface))]
    public class SingletonNetworkTablesInterface : INetworkTablesInterface
    {
        public event EventHandler<ConnectionChangedEventArgs> OnConnectionStatus;
        public event EventHandler OnDisconnect;
        public event EventHandler OnClientConnectionAttempt;

        public SingletonNetworkTablesInterface()
        {
            NetworkTablesExtensions.AddGlobalConnectionListenerOnSynchronizationContext(SynchronizationContext.Current,
                (connected) => OnConnectionStatus?.Invoke(this, new ConnectionChangedEventArgs { Connected = connected }), true);
        }

        public void ConnectToServer(string server, int port)
        {
            var inst = NetworkTableInstance.Default;
            inst.StartClient(server, port);
            OnClientConnectionAttempt?.Invoke(this, EventArgs.Empty);
        }

        public void ConnectToTeam(int team)
        {
            var inst = NetworkTableInstance.Default;
            inst.StartClientTeam(team);
            OnClientConnectionAttempt?.Invoke(this, EventArgs.Empty);
        }

        public NetworkTable GetTable(string path)
        {
            return NetworkTableInstance.Default.GetTable(path);
        }

        public void Disconnect()
        {
            NetworkTableInstance.Default.StopClient();
            OnDisconnect?.Invoke(this, EventArgs.Empty);
        }
    }
}
