using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    internal static class DashboardMetadataUtils
    {
        public static bool IsMatch(this IDashboardTypeMetadata data, string type)
        {
            return data.Type == type || data.IsWildCard();
        }

        public static bool IsWildCard(this IDashboardTypeMetadata data)
        {
            return data.Type == string.Empty;
        }
    }
}
