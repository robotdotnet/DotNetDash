using NetworkTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            NetworkTable.SetIPAddress(Server.Text);
            NetworkTable.SetClientMode();
            NetworkTable.Initialize();
            DialogResult = true;
            Close();
        }
    }
}
