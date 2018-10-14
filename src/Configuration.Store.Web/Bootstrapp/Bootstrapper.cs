using Autofac;
using Configuration.Store.Web.Bootstrapp.Modules;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Nancy.Swagger.Services;
using Swagger.ObjectModel;

namespace Configuration.Store.Web.Bootstrapp
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override ILifetimeScope GetApplicationContainer()
        {
            SwaggerMetadataProvider.SetInfo(
                "Configuration store Api documentation",
                "v0.1",
                "An Api to manage the configuration values for your application",
                new Contact
                {
                    Name = "Carlos Vicente",
                    EmailAddress = "carlosvicente200@gmail.com"
                });

            ApplicationPipelines
                .AfterRequest
                .AddItemToEndOfPipeline(x => x.Response.Headers.Add("Access-Control-Allow-Origin", "*"));

            var builder = new ContainerBuilder();

            builder.RegisterModule<ConfigStoreModule>();
            builder.RegisterModule<PersistenceModule>();
            builder.RegisterModule<JsonNetModule>();

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