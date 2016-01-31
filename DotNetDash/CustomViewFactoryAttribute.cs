using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CustomViewFactoryAttribute : ExportAttribute, ICustomViewFactoryMetadata
    {
        public CustomViewFactoryAttribute()
            :base(typeof(IViewProcessorFactory))
        {
        }

        public string Name { get; set; }
    }
}
