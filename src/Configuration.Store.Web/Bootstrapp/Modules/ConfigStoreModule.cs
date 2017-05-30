using Autofac;

namespace Configuration.Store.Web.Bootstrapp.Modules
{
    public class ConfigStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ConfigurationStoreService>()
                .As<IConfigurationStoreService>()
                .InstancePerLifetimeScope();
        }
    }
}