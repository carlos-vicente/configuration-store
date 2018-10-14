using Autofac;
using Configuration.Store.Web.Serialization;
using Nancy.Serialization.JsonNet;
using Newtonsoft.Json;

namespace Configuration.Store.Web.Bootstrapp.Modules
{
    public class JsonNetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var serializer = JsonSerializer.Create(JsonSerializationOptions.Settings);
            builder
                .RegisterInstance(serializer)
                .AsSelf();

            builder
                .Register(ctx => serializer)
                .As<JsonSerializer>();
        }
    }
}