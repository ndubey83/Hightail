using Autofac;
using Autofac.Core;
using NUnit.Framework;
using hems.HighTail;

namespace hems.DataFileDelivery.Test {
    public abstract class TestBaseSetup {
        protected IContainer Container;

        [SetUp]
        public virtual void Init() {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new HighTail.Logging.LogInjectionModule());
            HighTail.Logging.LogInjectionModule.SetLevel("NHibernate", "Info");
            Register(builder);
            Container = builder.Build();
        }

        public abstract void Register(ContainerBuilder builder);

        [TearDown]
        public void Cleanup() {
            if (Container != null) {
                Container.Dispose();
            }
        }
    }
}
