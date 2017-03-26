using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ploeh.AutoFixture;

namespace Configuration.Store.Persistence.Memory.Tests
{
    public class InMemoryConfigurationRepositoryTests
    {
        private InMemoryConfigurationRepository _sut;

        private readonly Fixture _fixture;

        public InMemoryConfigurationRepositoryTests()
        {
            _fixture = new Fixture();

            _sut = new InMemoryConfigurationRepository();
        }

        public async Task GetConfiguration_ShouldReturnNull_WhenNoConfigurationIsAvailable()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();

            // act
            var config = await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            // assert
            config.Should().BeNull();
        }
    }
}