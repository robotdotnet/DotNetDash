using NetworkTables;
using System.Windows;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for ServerConnectionWindow.xaml
    /// </summary>
    public partial class ServerConnectionWindow : Window
    {
        public ServerConnectionWindow()
        {
            InitializeComponent();
        }

        private void ConnectClicked(object sender, RoutedEventArgs e)
        {
            NetworkTable.Shutdown();
            NetworkTable.SetIPAddress(Properties.Settings.Default.LastServer.ToString());
            NetworkTable.SetClientMode();
            NetworkTable.Initialize();
            DialogResult = true;
            Close();
        }
    }
}
