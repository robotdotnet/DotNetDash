using System.Windows;
using NetworkTables;

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
            try
            {
                NetworkTable.Shutdown();
            }
            catch (System.Exception)
            {
            }
            NetworkTable.SetIPAddress(Properties.Settings.Default.LastServer.ToString());
            NetworkTable.SetClientMode();
            NetworkTable.Initialize();
            DialogResult = true;
            Close();
        }
    }
}
