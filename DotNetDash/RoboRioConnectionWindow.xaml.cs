using System.Windows;
using NetworkTables;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for RoboRioConnectionWindow.xaml
    /// </summary>
    public partial class RoboRioConnectionWindow : Window
    {
        private INetworkTablesInterface ntInterface;

        public RoboRioConnectionWindow()
        {
            InitializeComponent();
        }

        public RoboRioConnectionWindow(INetworkTablesInterface ntInterface)
            :this()
        {
            this.ntInterface = ntInterface;
        }

        private void ConnectClicked(object sender, RoutedEventArgs e)
        {
            ntInterface.Disconnect();
            ntInterface.ConnectToTeam(Properties.Settings.Default.TeamNumber);
            DialogResult = true;
            Close();
        }
    }
}
