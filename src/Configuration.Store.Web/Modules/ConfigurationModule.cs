using Nancy;

namespace Configuration.Store.Web.Modules
{
    public class ConfigurationModule: NancyModule
    {
        public ConfigurationModule()
        {
            Get["/"] = o => "OK";
        }
    }
}