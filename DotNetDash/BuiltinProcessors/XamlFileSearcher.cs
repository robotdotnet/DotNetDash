using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash.BuiltinProcessors
{
    public class XamlFileSearcher : IXamlSearcher
    {
        public IEnumerable<Stream> GetXamlDocumentStreams()
        {
            if (!Directory.Exists("Plugins")) yield break;
            foreach (var directory in new DirectoryInfo("Plugins").EnumerateDirectories())
            {
                foreach (var file in directory.EnumerateFiles("*.xaml"))
                {
                    yield return file.OpenRead();
                }
            }
        }
    }
}
