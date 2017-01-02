using System.Windows.Controls;

namespace DotNetDash
{
    /// <summary>
    /// Interaction logic for XamlView.xaml
    /// </summary>
    public partial class XamlView : UserControl
    {
        public XamlView()
        {
            InitializeComponent();
        }

        public string DashboardType { get; set; }
    }
}
