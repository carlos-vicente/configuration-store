using Nancy;

namespace Configuration.Store.Web.Modules.Ui
{
    public class ConfigurationAppNancyModule : NancyModule
    {
        public ConfigurationAppNancyModule()
        {
            Get["/"] = _ => this.Negotiate.WithView("ConfigurationStore");
        }
    }
}