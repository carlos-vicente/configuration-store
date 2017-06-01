using System;
using Common.Tests;
using Configuration.Store.Web.Bootstrapp;
using FluentAssertions;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Ploeh.AutoFixture;

namespace Configuration.Store.Web.Tests.Modules.Api
{
    public class ConfigurationNancyModuleTests
    {
        private readonly Browser _browser;
        private readonly Fixture _fixture;

        public ConfigurationNancyModuleTests()
        {
            var bootstrapper = new Bootstrapper();
            _browser = new Browser(bootstrapper);
            _fixture = new Fixture();
            _fixture.Customizations.Add(new VersionBuilder());
        }

        public void SomeTest()
        {
            // arrange
            var configKey = _fixture.Create<string>();
            var configVersion = _fixture.Create<Version>();
            var envTag = _fixture.Create<string>();

            // act
            var response = _browser.Get(
                $"/api/{configKey}/{configVersion}/{envTag}",
                with =>
                {
                    with.HttpRequest();
                    with.Accept(new MediaRange("application/json"));
                });

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}