using DotNetDash.BuiltinProcessors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDash.Test
{
    public class TestBase
    {
        protected CompositionContainer composer;

        [TestFixtureSetUp]
        public void Setup()
        {
            var builder = new RegistrationBuilder();
            builder.ForType<MockXamlSearcher>().Export<IXamlSearcher>();
            composer = new CompositionContainer(new AggregateCatalog(
                new AssemblyCatalog(typeof(MainWindow).Assembly, builder), new AssemblyCatalog(typeof(TestBase).Assembly, builder)));
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            composer.Dispose();
        }
    }
}
