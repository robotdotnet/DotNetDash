using System.Windows.Controls;

namespace DotNetDash.SpeedController
{
    /// <summary>
    /// Interaction logic for CANView.xaml
    /// </summary>
    public partial class CANView : UserControl
    {
        public CANView()
        {
            InitializeComponent();
            ViewSelector.ResourceHost = this;
        }
    }
}
