using System.Threading;
using System.Threading.Tasks;
using Nancy;

namespace Configuration.Store.Web.Modules
{
    public class ConfigurationModule: NancyModule
    {
        private readonly IConfigurationStoreService _configStoreService;

        public ConfigurationModule(IConfigurationStoreService configStoreService)
        {
            _configStoreService = configStoreService;

            Get["GetConfigForVersion", "/{configKey}/{configVersion}", true] = GetConfigForVersion;
        }

        private async Task<dynamic> GetConfigForVersion(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            string version = parameters.configVersion;

            int? currentSequence = this.Request.Query.seq;

            var configuration = await _configStoreService
                .GetConfiguration(key, version, currentSequence)
                .ConfigureAwait(false);

            var statusCode = HttpStatusCode.OK;
            var result = configuration;

            if (configuration == null)
            {
                statusCode = HttpStatusCode.NotFound;
            }
            else
            {
                if (currentSequence.HasValue
                    && configuration.Sequence == currentSequence)
                {
                    // no need to return has nothing changed
                    result = null;
                    statusCode = HttpStatusCode.NotModified;
                }
            }

            return Negotiate
                .WithStatusCode(statusCode)
                .WithModel(result);
        }
    }
}