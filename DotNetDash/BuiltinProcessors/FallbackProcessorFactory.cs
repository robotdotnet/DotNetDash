using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkTables.Tables;
using System.ComponentModel.Composition;
using System.Windows.Markup;

namespace DotNetDash.BuiltinProcessors
{
    /// <summary>
    /// This factory attempts to load a XAML document from the Extensions subdirectories and use a processor on that to create the view.
    /// If this a suitable XAML file is not found, then it will fallback to a processor that just creates text boxes with labels.
    /// </summary>
    [DashboardType(typeof(ITableProcessorFactory), "")]
    public sealed class FallbackProcessorFactory : ITableProcessorFactory
    {
        private IXamlSearcher searcher;
        
        [ImportingConstructor]
        public FallbackProcessorFactory(IXamlSearcher searcher)
        {
            this.searcher = searcher;
        }

        public TableProcessor Create(string subTable, ITable table, CompositionContainer container)
        {
            var xamlViews = LoadXamlDocs();
            var matchingView = xamlViews.FirstOrDefault(view => view.DashboardType == table.GetString("~TYPE~", ""));
            return matchingView != null ? CreateProcessorForFirstView(subTable, table, container, matchingView) :
                (TableProcessor)new DefaultProcessor(subTable, table, container);
        }

        private static XamlProcessor CreateProcessorForFirstView(string subTable, ITable table, CompositionContainer container, XamlView view)
        {
            return new XamlProcessor(subTable, table, container, view);
        }

        private IEnumerable<XamlView> LoadXamlDocs()
        {
            var views = new List<XamlView>();
            foreach (var stream in searcher.GetXamlDocumentStreams())
            {
                try
                {
                    using (stream)
                    {
                        views.Add((XamlView)XamlReader.Load(stream));
                    }
                }
                catch (InvalidCastException)
                {
                    //TODO: log invalid casts
                }
                catch (XamlParseException)
                {
                    //TODO: log xaml parsing errors
                }
            }
            return views;
        }
    }
}
