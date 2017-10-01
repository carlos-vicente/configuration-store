using Autofac;
using Configuration.Store.Web.Bootstrapp.Modules;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;

namespace Configuration.Store.Web.Bootstrapp
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigStoreModule>();
            builder.RegisterModule<PersistenceModule>();

            return builder.Build();
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions.AddDirectory("Scripts");
            nancyConventions.StaticContentsConventions.AddDirectory("Styles");
        }
    }
}