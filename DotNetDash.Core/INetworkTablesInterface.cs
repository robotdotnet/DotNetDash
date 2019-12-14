using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRC.NetworkTables;

namespace DotNetDash
{
    public interface INetworkTablesInterface
    {
        void ConnectToTeam(int team);
        void ConnectToServer(string server, int port);
        void Disconnect();
        event EventHandler OnClientConnectionAttempt;
        event EventHandler OnDisconnect;
        event EventHandler<ConnectionChangedEventArgs> OnConnectionStatus;
        NetworkTable GetTable(string path);
    }

    public class ConnectionChangedEventArgs : EventArgs
    {
        public bool Connected { get; set; }
    }


    public static class NetworkTableInterfaceExtensions
    {
        public static void ConnectToServer(this INetworkTablesInterface nt, string server)
        {
            nt.ConnectToServer(server, NetworkTableInstance.DefaultPort);
        }
    }
}
