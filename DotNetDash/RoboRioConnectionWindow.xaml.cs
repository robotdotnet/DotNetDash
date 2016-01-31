using NetworkTables;
using System.Windows;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for RoboRioConnectionWindow.xaml
    /// </summary>
    public partial class RoboRioConnectionWindow : Window
    {
        public RoboRioConnectionWindow()
        {
            InitializeComponent();
        }

        private void ConnectClicked(object sender, RoutedEventArgs e)
        {
            NetworkTable.Shutdown();
            NetworkTable.SetIPAddress($"roborio-{Properties.Settings.Default.TeamNumber}-frc.local");
            NetworkTable.SetClientMode();
            NetworkTable.Initialize();
            DialogResult = true;
            Close();
        }
    }
}
