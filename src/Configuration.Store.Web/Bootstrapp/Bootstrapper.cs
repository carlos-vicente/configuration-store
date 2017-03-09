using Autofac;
using Nancy.Bootstrappers.Autofac;

namespace Configuration.Store.Web.Bootstrapp
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override ILifetimeScope GetApplicationContainer()
        {
            var builder = new ContainerBuilder();

            return builder.Build();
        }
    }
}