using System.Windows.Controls;

namespace DotNetDash.CANSpeedController
{
    /// <summary>
    /// Interaction logic for CANView.xaml
    /// </summary>
    public partial class CANView : UserControl
    {
        public CANView()
        {
            InitializeComponent();
            Selector.ResourceHost = this;
        }
    }
}
