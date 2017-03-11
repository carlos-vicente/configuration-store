using Autofac;
using Configuration.Store.Persistence;
using Configuration.Store.Persistence.Memory;

namespace Configuration.Store.Web.Bootstrapp.Modules
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<InMemoryConfigurationRepository>()
                .As<IConfigurationRepository>()
                .SingleInstance();
        }
    }
}