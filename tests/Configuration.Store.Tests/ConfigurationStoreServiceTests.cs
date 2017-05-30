using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration.Store.Persistence;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Fixture = Ploeh.AutoFixture.Fixture;

namespace Configuration.Store.Tests
{
    public class ConfigurationStoreServiceTests
    {
        private readonly IConfigurationRepository _repository;
        private readonly ConfigurationStoreService _sut;

        private readonly Fixture _fixture;

        public ConfigurationStoreServiceTests(
            IConfigurationRepository repo,
            ConfigurationStoreService sut)
        {
            _fixture = new Fixture();
            _repository = repo;
            _sut = sut;
        }

        public async Task GetConfiguration_ShouldReturnCorrectConfiguration_WhenAvailableForEnvironmentTag()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var tag = _fixture.Create<string>();

            var storedConfig = _fixture
                .Build<StoredConfig>()
                .With(sc => sc.Type, _fixture.Create<ConfigurationDataType>().ToString())
                .With(sc => sc.Values, new List<StoredConfigValues>
                {
                    _fixture
                        .Build<StoredConfigValues>()
                        .With(scv => scv.EnvironmentTags, new List<string> {tag})
                        .Create()
                })
                .Create();

            var expectedConfig = new Configuration
            {
                Type = (ConfigurationDataType) Enum.Parse(typeof(ConfigurationDataType), storedConfig.Type),
                Sequence = storedConfig.Values.Single().Sequence,
                Data = storedConfig.Values.Single().Data
            };

            A.CallTo(() => _repository.GetConfiguration(key, version))
                .Returns(storedConfig);

            // act
            var config = await _sut
                .GetConfiguration(key, version, tag, null)
                .ConfigureAwait(false);

            // assert
            config.ShouldBeEquivalentTo(expectedConfig);
        }

        public async Task GetConfiguration_ShouldReturnNull_WhenNotAvailableForEnvironmentTag()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var tag = _fixture.Create<string>();
            var someOtherTag = _fixture.Create<string>();

            var storedConfig = _fixture
                .Build<StoredConfig>()
                .With(sc => sc.Type, _fixture.Create<ConfigurationDataType>().ToString())
                .With(sc => sc.Values, new List<StoredConfigValues>
                {
                    _fixture
                        .Build<StoredConfigValues>()
                        .With(scv => scv.EnvironmentTags, new List<string> {tag})
                        .Create()
                })
                .Create();

            A.CallTo(() => _repository.GetConfiguration(key, version))
                .Returns(storedConfig);

            // act
            var actual = await _sut
                .GetConfiguration(key, version, someOtherTag, null)
                .ConfigureAwait(false);

            // assert
            actual.Should().BeNull();
        }
    }
}