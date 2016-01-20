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
            yield return CreateValidMarkupStream();

            yield return CreateInvalidMarkupStream();
        }

        private static MemoryStream CreateValidMarkupStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@"
<dash:XamlView
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml""
            xmlns:dash = ""clr-namespace:DotNetDash;assembly=DotNetDash""
            DashboardType =""TestType"">
<StackPanel Orientation=""Horizontal"">
    <Label>Current Value</Label>
    <TextBox Text=""{Binding [Value]}"" />
</StackPanel>
</dash:XamlView>
");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private static MemoryStream CreateInvalidMarkupStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@"
<dash:XamlView
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml""
            xmlns:dash = ""clr-namespace:DotNetDash;assembly=DotNetDash""
            DashboardType =""InvalidMarkupType"">
<StackPanel Orientation=""Horizontal"">
    <Label>Current Value</Label>
    <TextBox Text=""{Binding [Value]}"" />

</dash:XamlView>
"); //Lacks the closing tag for StackPanel
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
