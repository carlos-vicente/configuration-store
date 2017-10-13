using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Configuration.Store.Web.Contracts.Requests;
using Configuration.Store.Web.Utils;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;

namespace Configuration.Store.Web.Modules.Api
{
    public class ConfigurationNancyModule: NancyModule
    {
        private static readonly MediaRange JsonMediaRange = new MediaRange("application/json");

        private readonly IConfigurationStoreService _configStoreService;

        public ConfigurationNancyModule(IConfigurationStoreService configStoreService)
        {
            _configStoreService = configStoreService;

            Get[RouteRegistry.Api.Configuration.GetConfigForVersion.Name,
                RouteRegistry.Api.Configuration.GetConfigForVersion.Template,
                true] = GetConfigForVersion;
            Put[RouteRegistry.Api.Configuration.AddNewConfiguration.Name,
                RouteRegistry.Api.Configuration.AddNewConfiguration.Template,
                true] = AddNewConfiguration;
            Put[RouteRegistry.Api.Configuration.AddNewValueToConfiguration.Name,
                RouteRegistry.Api.Configuration.AddNewValueToConfiguration.Template,
                true] = AddNewValueToConfiguration;
            Put[RouteRegistry.Api.Configuration.UpdateValueOnConfiguration.Name,
                RouteRegistry.Api.Configuration.UpdateValueOnConfiguration.Template,
                true] = UpdateValueOnConfiguration;
            Delete[RouteRegistry.Api.Configuration.DeleteValueFromConfiguration.Name,
                RouteRegistry.Api.Configuration.DeleteValueFromConfiguration.Template,
                true] = DeleteValueFromConfiguration;
            Delete[RouteRegistry.Api.Configuration.DeleteConfiguration.Name,
                RouteRegistry.Api.Configuration.DeleteConfiguration.Template,
                true] = DeleteConfiguration;
        }

        private async Task<dynamic> GetConfigForVersion(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;
            string environmentTag = parameters.envTag;

            int? currentSequence = Request.Query.seq;

            var configuration = await _configStoreService
                .GetConfiguration(key, version, environmentTag, currentSequence)
                .ConfigureAwait(false);

            var negociator = Negotiate
                .WithStatusCode(HttpStatusCode.OK);

            if (configuration == null)
            {
                negociator = Negotiate
                    .WithStatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                if (currentSequence.HasValue
                    && configuration.Sequence == currentSequence)
                {
                    // no need to return, nothing changed
                    negociator = Negotiate
                        .WithStatusCode(HttpStatusCode.NotModified);
                }
                else
                {
                    negociator = negociator
                        .WithModel(configuration);
                }
            }

            return negociator
                .WithAllowedMediaRange(JsonMediaRange);
        }

        private async Task<dynamic> AddNewConfiguration(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;

            // TODO: BindAndValidate with FluentValidation
            var request = this.Bind<NewConfigurationRequest>(new BindingConfig
            {
                BodyOnly = true,
                IgnoreErrors = false
            });

            var negociator = Negotiate
                .WithAllowedMediaRange(JsonMediaRange);

            if (request == null)
                return negociator.WithStatusCode(HttpStatusCode.BadRequest);

            var version = request.Version.ToVersion();

            await _configStoreService
                .AddConfiguration(key, version, request.Type)
                .ConfigureAwait(false);

            var location = $"{this.Context.Request.Url}/{version}";

            return negociator
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Location", location);
        }

        private async Task<dynamic> UpdateValueOnConfiguration(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;
            Guid valueId = parameters.valueId;

            // TODO: BindAndValidate with FluentValidation
            var request = this.Bind<UpdateValueOnConfigurationRequest>(new BindingConfig
            {
                BodyOnly = true,
                IgnoreErrors = false
            });

            var negociator = Negotiate
                .WithAllowedMediaRange(JsonMediaRange);

            if (request == null)
                return negociator.WithStatusCode(HttpStatusCode.BadRequest);

            await _configStoreService
                .UpdateValueOnConfiguration(key, version, valueId, request.Tags, request.Value)
                .ConfigureAwait(false);

            return negociator.WithStatusCode(HttpStatusCode.OK);
        }

        private async Task<dynamic> AddNewValueToConfiguration(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;

            // TODO: BindAndValidate with FluentValidation
            var request = this.Bind<NewValueToConfigurationRequest>(new BindingConfig
            {
                BodyOnly = true,
                IgnoreErrors = false
            });

            var negociator = Negotiate
                .WithAllowedMediaRange(JsonMediaRange);

            if (request == null)
                return negociator.WithStatusCode(HttpStatusCode.BadRequest);

            var valueId = await _configStoreService
                .AddValueToConfiguration(key, version, request.Tags, request.Value)
                .ConfigureAwait(false);

            var location = $"{this.Context.Request.Url}/{valueId}";

            return negociator
                .WithStatusCode(HttpStatusCode.OK)
                .WithHeader("Location", location);
        }

        private async Task<dynamic> DeleteValueFromConfiguration(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;
            Guid valueId = parameters.valueId;

            var negociator = Negotiate
                .WithAllowedMediaRange(JsonMediaRange);

            await _configStoreService
                .RemoveValueFromConfiguration(key, version, valueId)
                .ConfigureAwait(false);

            return negociator
                .WithStatusCode(HttpStatusCode.OK);
        }

        private async Task<dynamic> DeleteConfiguration(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;

            var negociator = Negotiate
                .WithAllowedMediaRange(JsonMediaRange);

            await _configStoreService
                .RemoveConfiguration(key, version)
                .ConfigureAwait(false);

            return negociator
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}