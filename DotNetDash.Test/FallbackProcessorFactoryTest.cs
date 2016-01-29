using DotNetDash.BuiltinProcessors;
using NetworkTables.Tables;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetDash.Test
{
    [TestFixture, RequiresSTA]
    public class FallbackProcessorFactoryTest
    {
        [Test]
        public void FactoryLoadsXamlParserWhenXamlDocumentOfTypeExists()
        {
            ITable baseTable = new MockTable();
            var fallbackProcessorFactory = new FallbackProcessorFactory(new MockXamlSearcher(), Enumerable.Empty<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>>());
            baseTable.PutString("~TYPE~", "TestType");
            baseTable.PutString("Value", "a value");
            Assert.IsAssignableFrom<XamlProcessor>(fallbackProcessorFactory.Create("Test Table", baseTable));
        }

        [Test]
        public void FactoryLoadsDefaultParserWhenXamlDocumentOfTypeDoesNotExist()
        {
            ITable baseTable = new MockTable();
            var fallbackProcessorFactory = new FallbackProcessorFactory(new MockXamlSearcher(), Enumerable.Empty<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>>());
            baseTable.PutString("~TYPE~", "NonExistentType");
            baseTable.PutString("Value", "a value");
            Assert.IsAssignableFrom<DefaultProcessor>(fallbackProcessorFactory.Create("Test Table", baseTable));
        }
        [Test]
        public void FactoryLoadsDefaultParserWhenInvalidXamlDocumentOfTypeExists()
        {
            ITable baseTable = new MockTable();
            var fallbackProcessorFactory = new FallbackProcessorFactory(new MockXamlSearcher(), Enumerable.Empty<Lazy<ITableProcessorFactory, IDashboardTypeMetadata>>());
            baseTable.PutString("~TYPE~", "InvalidMarkupType");
            baseTable.PutString("Value", "a value");
            Assert.IsAssignableFrom<DefaultProcessor>(fallbackProcessorFactory.Create("Test Table", baseTable));
        }
    }
}
