using System;
using System.Threading;
using System.Threading.Tasks;
using Configuration.Store.Web.Contracts.Requests;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using AutoMapper;
using Configuration.Store.Web.Contracts.Responses;
using System.Collections.Generic;

namespace Configuration.Store.Web.Modules.Api
{
    public class ConfigurationNancyModule: NancyModule
    {
        private static readonly MediaRange JsonMediaRange = new MediaRange("application/json");

        private readonly IConfigurationStoreService _configStoreService;

        public ConfigurationNancyModule(IConfigurationStoreService configStoreService)
        {
            _configStoreService = configStoreService;

            Get[RouteRegistry.Api.Configuration.GetConfigs.Name,
                RouteRegistry.Api.Configuration.GetConfigs.Template,
                true] = GetConfigs;
            Get[RouteRegistry.Api.Configuration.GetConfigForVersion.Name,
                RouteRegistry.Api.Configuration.GetConfigForVersion.Template,
                true] = GetConfigForVersion;
            Put[RouteRegistry.Api.Configuration.AddNewConfiguration.Name,
                RouteRegistry.Api.Configuration.AddNewConfiguration.Template,
                true] = AddNewConfiguration;
            Put[RouteRegistry.Api.Configuration.AddNewValueToConfiguration.Name,
                RouteRegistry.Api.Configuration.AddNewValueToConfiguration.Template,
                true] = AddNewValueToConfiguration;
            Post[RouteRegistry.Api.Configuration.UpdateValueOnConfiguration.Name,
                RouteRegistry.Api.Configuration.UpdateValueOnConfiguration.Template,
                true] = UpdateValueOnConfiguration;
            Delete[RouteRegistry.Api.Configuration.DeleteValueFromConfiguration.Name,
                RouteRegistry.Api.Configuration.DeleteValueFromConfiguration.Template,
                true] = DeleteValueFromConfiguration;
            Delete[RouteRegistry.Api.Configuration.DeleteConfiguration.Name,
                RouteRegistry.Api.Configuration.DeleteConfiguration.Template,
                true] = DeleteConfiguration;
        }

        private async Task<dynamic> GetConfigs(dynamic parameters, CancellationToken token)
        {
            var configurationKeys = await _configStoreService
                .GetConfigurationKeys()
                .ConfigureAwait(false);

            var mappedKeys = Mapper
                .Map<IEnumerable<ConfigurationKey>, IEnumerable<ConfigKeyListItem>>(configurationKeys);

            return Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithAllowedMediaRange(JsonMediaRange)
                .WithModel(mappedKeys);
        }

        private async Task<dynamic> GetConfigForVersion(dynamic parameters, CancellationToken token)
        {
            string key = parameters.configKey;
            Version version = parameters.configVersion;
            string environmentTag = parameters.envTag;

            int? currentSequence = Request.Query.seq;

            var configurationValue = await _configStoreService
                .GetConfigurationValue(key, version, environmentTag)
                .ConfigureAwait(false);

            var negociator = Negotiate
                .WithStatusCode(HttpStatusCode.OK);

            if (configurationValue == null)
            {
                negociator = Negotiate
                    .WithStatusCode(HttpStatusCode.NotFound);
            }
            else
            {
                if (currentSequence.HasValue
                    && configurationValue.Sequence == currentSequence.Value)
                {
                    // no need to return, nothing changed
                    negociator = Negotiate
                        .WithStatusCode(HttpStatusCode.NotModified);
                }
                else
                {
                    var mappedConfigValue = Mapper.Map<ConfigurationValue, ConfigValueListItem>(configurationValue);
                    negociator = negociator
                        .WithModel(mappedConfigValue);
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

            await _configStoreService
                .AddConfiguration(key, request.Type)
                .ConfigureAwait(false);

            var location = $"{this.Context.Request.Url}";

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
                .RemoveConfiguration(key)
                .ConfigureAwait(false);

            return negociator
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}