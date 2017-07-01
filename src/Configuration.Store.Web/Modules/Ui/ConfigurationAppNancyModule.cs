using Configuration.Store.Web.Models;
using Configuration.Store.Web.Models.Shared;
using Nancy;
using System.Threading;
using System.Threading.Tasks;

namespace Configuration.Store.Web.Modules.Ui
{
    public class ConfigurationAppNancyModule : NancyModule
    {
        private readonly IConfigurationStoreService _service;

        public ConfigurationAppNancyModule(IConfigurationStoreService service)
        {
            _service = service;

            Get[RouteRegistry.Ui.Configuration.GetHome, true] = GetHome;
        }

        private async Task<dynamic> GetHome(dynamic parameters, CancellationToken token)
        {
            var configKeys = await _service
                .GetConfigurationKeys()
                .ConfigureAwait(false);

            return Negotiate
                .WithModel(new ReactViewModel<HomeViewModel>
                {
                    ComponentName = "ConfigurationList",
                    ContainerHtmlId = "root",
                    Data = new HomeViewModel
                    {
                        configKeys = configKeys
                    }
                })
                .WithView("ConfigurationStore");
        }
    }
}