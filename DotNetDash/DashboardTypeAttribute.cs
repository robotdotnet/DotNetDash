using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DashboardTypeAttribute : ExportAttribute, IDashboardTypeMetadata
    {
        public DashboardTypeAttribute(Type exportType, string type)
            :base(exportType)
        {
            Type = type;
            Builtin = false;
        }

        internal DashboardTypeAttribute(Type exportType, string type, bool builtin)
            :base(exportType)
        {
            Type = type;
            Builtin = builtin;
        }

        public bool Builtin { get; }

        public string Type { get; }
    }
}
