using System;
using System.Threading.Tasks;
using Common.Tests;
using Configuration.Store.Web.Modules.Api;
using FakeItEasy;
using FluentAssertions;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Ploeh.AutoFixture;
using Configuration.Store.Web.Contracts.Responses;
using AutoMapper;
using Configuration.Store.Web.Contracts.Responses.Mapping;

namespace Configuration.Store.Web.Tests.Modules.Api
{
    public class ConfigurationNancyModuleTests
    {
        private readonly Browser _browser;
        private readonly Fixture _fixture;

        private const string JsonMediaType = "application/json";

        private readonly IConfigurationStoreService _configurationStoreService;

        static ConfigurationNancyModuleTests()
        {
            Mapper.Reset();
            Mapper.Initialize(config => config.AddProfile<ConfigKeyDetailMappingProfile>());
        }

        public ConfigurationNancyModuleTests(
            IConfigurationStoreService configurationStoreService)
        {
            _configurationStoreService = configurationStoreService;

            _browser = new Browser(configurator =>
            {
                configurator.Module<ConfigurationNancyModule>();
                configurator.Dependency(_configurationStoreService);
            });
            _fixture = new Fixture();
            _fixture.Customizations.Add(new VersionBuilder());
        }

        public void GetConfigForVersion_ShouldReturnNotFound_WhenNoConfigurationExists()
        {
            // arrange
            var configKey = _fixture.Create<string>();
            var configVersion = _fixture.Create<Version>();
            var envTag = _fixture.Create<string>();

            A.CallTo(() => _configurationStoreService.GetConfigurationValue(
                    configKey,
                    configVersion,
                    envTag))
                .Returns(Task.FromResult<ConfigurationValue>(null));

            // act
            var response = _browser.Get(
                $"/api/keys/{configKey}/version/{configVersion}/{envTag}",
                with =>
                {
                    with.HttpRequest();
                    with.Accept(new MediaRange("application/json"));
                });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        public void GetConfigForVersion_ShouldReturnNotModified_WhenConfigurationExistsButHasNotChangedSinceLatestGet()
        {
            // arrange
            var configKey = _fixture.Create<string>();
            var configVersion = _fixture.Create<Version>();
            var envTag = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();

            var config = _fixture
                .Build<ConfigurationValue>()
                .With(configuration => configuration.Sequence, sequence)
                .Create();

            A.CallTo(() => _configurationStoreService.GetConfigurationValue(
                    configKey,
                    configVersion,
                    envTag))
                .Returns(config);

            // act
            var response = _browser.Get(
                $"/api/keys/{configKey}/version/{configVersion}/{envTag}",
                with =>
                {
                    with.HttpRequest();
                    with.Accept(new MediaRange(JsonMediaType));
                    with.Query("seq", sequence.ToString());
                });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotModified);
        }

        public void GetConfigForVersion_ShouldReturnConfiguration_WhenConfigurationExists()
        {
            // arrange
            var configKey = _fixture.Create<string>();
            var configVersion = _fixture.Create<Version>();
            var envTag = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();

            var config = _fixture
                .Build<ConfigurationValue>()
                .With(configuration => configuration.Sequence, sequence)
                .Create();

            A.CallTo(() => _configurationStoreService.GetConfigurationValue(
                    configKey,
                    configVersion,
                    envTag))
                .Returns(config);

            var expectedConfig = new ConfigValueListItem
            {
                Id = config.Id.ToString(),
                Version = config.Version.ToString(),
                Data = config.Data,
                Sequence = config.Sequence,
                EnvironmentTags = config.EnvironmentTags,
                CreatedAt = config.CreatedAt
            };

            // act
            var response = _browser.Get(
                $"/api/keys/{configKey}/version/{configVersion}/{envTag}",
                with =>
                {
                    with.HttpRequest();
                    with.Accept(new MediaRange(JsonMediaType));
                });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body
                .DeserializeJson<ConfigValueListItem>()
                .ShouldBeEquivalentTo(expectedConfig);
        }

        public void GetConfigForVersion_ShouldReturnConfiguration_WhenConfigurationExistsWithHigherSequence()
        {
            // arrange
            var configKey = _fixture.Create<string>();
            var configVersion = _fixture.Create<Version>();
            var envTag = _fixture.Create<string>();
            var previousSequence = _fixture.Create<int>();
            var sequence = previousSequence + _fixture.Create<int>();

            var config = _fixture
                .Build<ConfigurationValue>()
                .With(configuration => configuration.Sequence, sequence)
                .Create();

            A.CallTo(() => _configurationStoreService.GetConfigurationValue(
                    configKey,
                    configVersion,
                    envTag))
                .Returns(Task.FromResult(config));

            var expectedConfig = new ConfigValueListItem
            {
                Id = config.Id.ToString(),
                Version = config.Version.ToString(),
                Data = config.Data,
                Sequence = config.Sequence,
                EnvironmentTags = config.EnvironmentTags,
                CreatedAt = config.CreatedAt
            };

            // act
            var response = _browser.Get(
                $"/api/keys/{configKey}/version/{configVersion}/{envTag}",
                with =>
                {
                    with.HttpRequest();
                    with.Accept(new MediaRange(JsonMediaType));
                    with.Query("seq", previousSequence.ToString());
                });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Body
                .DeserializeJson<ConfigValueListItem>()
                .ShouldBeEquivalentTo(expectedConfig);
        }
    }
}