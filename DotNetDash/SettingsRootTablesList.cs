using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    [Export(typeof(IRootTablesList))]
    public class SettingsRootTablesList : IRootTablesList
    {
        public IEnumerable<string> RootTables => Properties.Settings.Default.RootTables.Cast<string>();
    }
}
