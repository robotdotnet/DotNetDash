using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    [Export(typeof(IConnectionPrompts))]
    public class WpfConnectionPrompts : IConnectionPrompts
    {
        public string PromptServerName()
        {
            return new ServerConnectionWindow().ShowDialog() == true ? Properties.Settings.Default.LastServer : null;
        }

        public int? PromptTeamNumber()
        {
            return new RoboRioConnectionWindow().ShowDialog() == true ? Properties.Settings.Default.TeamNumber : (int?)null;
        }
    }
}
