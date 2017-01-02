using System.ComponentModel.Composition;
using System.Windows;
using NetworkTables;

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
            DialogResult = true;
            Close();
        }
    }
}
