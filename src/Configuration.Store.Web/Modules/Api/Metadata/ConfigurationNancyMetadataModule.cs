﻿using Configuration.Store.Web.Contracts.Requests;
using Nancy;
using Nancy.Swagger;
using Nancy.Swagger.Services;
using Nancy.Swagger.Services.RouteUtils;
using Swagger.ObjectModel;

namespace Configuration.Store.Web.Modules.Api.Metadata
{
    public class ConfigurationNancyMetadataModule : Nancy.Swagger.Modules.SwaggerMetadataModule
    {
        public ConfigurationNancyMetadataModule(
            ISwaggerModelCatalog modelCatalog,
            ISwaggerTagCatalog tagCatalog)
            : base(modelCatalog, tagCatalog)
        {
            var configurationStoreTag = new Tag
            {
                Name = "Configuration",
                Description = "Operations to manage configuration values"
            };

            RouteDescriber
                .AddAdditionalModels(typeof(ValueType));

            RouteDescriber.DescribeRouteWithParams<Configuration>(
                RouteRegistry.Api.Configuration.GetConfigForVersion.Name,
                "Gets the latest configuration value for the environment tag specified in the defined version",
                "Gets configuration value for environment",
                new[]
                {
                    new HttpResponseMetadata<Configuration>
                    {
                        Code = (int)HttpStatusCode.OK,
                        Message = "OK"
                    },
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.NotFound,
                        Message = "Configuration key or value for environment not found"
                    },
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.NotModified,
                        Message = "Configuration value found for environment, but no change from client version"
                    }
                },
                new[]
                {
                    new Parameter
                    {
                        Name = "configKey",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration's key",
                        Type = "string"
                    },
                    new Parameter
                    {
                        Name = "configVersion:version",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration's value version (semantic version format)",
                        Type = "string"
                    },
                    new Parameter
                    {
                        Name = "envTag",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration's environment",
                        Type = "string"
                    }
                },
                new[]
                {
                    configurationStoreTag
                });

            RouteDescriber.DescribeRouteWithParams(
                RouteRegistry.Api.Configuration.AddNewConfiguration.Name,
                "Adds a new configuration key with a given value type",
                "Add a new configuration key",
                new[]
                {
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.OK,
                        Message = "OK"
                    },
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Request received does not match expectation"
                    }
                },
                new[]
                {
                    new Parameter
                    {
                        Name = "configKey",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration's key",
                        Type = "string"
                    },
                    new BodyParameter<NewConfigurationRequest>(modelCatalog)
                    {
                        Name = "new configuration",
                        Required = true,
                        Description = "The configuration definition"
                    }
                },
                new[]
                {
                    configurationStoreTag
                });

            RouteDescriber.DescribeRouteWithParams(
                RouteRegistry.Api.Configuration.AddNewValueToConfiguration.Name,
                "Adds a new configuration value to an existing key, used to add values to a different set of environments",
                "Add a new configuration value for the key",
                new[]
                {
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.OK,
                        Message = "OK"
                    },
                    new HttpResponseMetadata
                    {
                        Code = (int)HttpStatusCode.BadRequest,
                        Message = "Request received does not match expectation"
                    }
                },
                new[]
                {
                    new Parameter
                    {
                        Name = "configKey",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration key",
                        Type = "string"
                    },
                    new Parameter
                    {
                        Name = "configVersion:version",
                        In = ParameterIn.Path,
                        Required = true,
                        Description = "The configuration key's version",
                        Type = "string"
                    },
                    new BodyParameter<NewValueToConfigurationRequest>(modelCatalog)
                    {
                        Name = "new configuration value",
                        Required = true,
                        Description = "The configuration value definition"
                    }
                },
                new[]
                {
                    configurationStoreTag
                });
        }
    }
}