using AutoMapper;
using Configuration.Store.Web.Contracts.Responses;
using Configuration.Store.Web.Views.Models;
using Nancy;
using System.Collections.Generic;
using System.Linq;
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

            Get[RouteRegistry.Ui.Configuration.GetHome, true] = GetConfigurationList;
            Get[RouteRegistry.Ui.Configuration.GetConfiguration, true] = GetConfiguration;
        }

        private async Task<dynamic> GetConfigurationList(dynamic parameters, CancellationToken token)
        {
            var configKeys = await _service
                .GetConfigurationKeys()
                .ConfigureAwait(false);

            var orderedConfigKeys = configKeys.OrderBy(k => k.Key);

            return Negotiate
                .WithModel(new ConfigurationListViewModel
                {
                    ConfigKeys = Mapper
                        .Map<IEnumerable<ConfigurationKey>, IEnumerable<ConfigKeyListItem>>(orderedConfigKeys)
                })
                .WithView("ConfigurationList");
        }

        private async Task<dynamic> GetConfiguration(dynamic parameters, CancellationToken token)
        {
            string configKey = parameters.configKey;

            var key = await _service
                .GetConfigurationKey(configKey)
                .ConfigureAwait(false);

            var viewModel = new ConfigurationKeyViewModel();

            if(key != null)
            {
                viewModel.Detail = Mapper.Map<ConfigurationKey, ConfigKeyDetailed>(key);
            }

            return Negotiate
                .WithModel(viewModel)
                .WithView("ConfigurationKey");
        }
    }
}