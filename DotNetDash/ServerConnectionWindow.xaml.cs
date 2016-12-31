using System.Windows;
using NetworkTables;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for ServerConnectionWindow.xaml
    /// </summary>
    public partial class ServerConnectionWindow : Window
    {
        private INetworkTablesInterface ntInterface;

        public ServerConnectionWindow()
        {
            InitializeComponent();
        }

        public ServerConnectionWindow(INetworkTablesInterface ntInterface)
            :this()
        {
            this.ntInterface = ntInterface;
        }

        private void ConnectClicked(object sender, RoutedEventArgs e)
        {
            ntInterface.Disconnect();
            ntInterface.ConnectToServer(Properties.Settings.Default.LastServer.ToString());
            DialogResult = true;
            Close();
        }
    }
}
