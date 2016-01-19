using DotNetDash.BuiltinProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DotNetDash.Test
{
    public class MockXamlSearcher : IXamlSearcher
    {
        public IEnumerable<Stream> GetXamlDocumentStreams()
        {
            throw new NotImplementedException();
        }
    }
}
