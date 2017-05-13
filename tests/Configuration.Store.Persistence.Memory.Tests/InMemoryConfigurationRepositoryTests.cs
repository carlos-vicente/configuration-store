using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Tests;
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
            _fixture.Customizations.Add(new VersionBuilder());
        }

        public async Task GetConfiguration_ShouldReturnNull_WhenNoConfigurationIsAvailable()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();

            _sut = new InMemoryConfigurationRepository();

            // act
            var config = await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            // assert
            config.Should().BeNull();
        }

        public async Task GetConfiguration_ShouldReturnNull_WhenKeyIsAvailableButVersionIsNot()
        {
            // arrange
            var key = _fixture.Create<string>();
            var versionToQuery = _fixture.Create<Version>();
            var versionInStore = _fixture.Create<Version>();

            _sut =
                new InMemoryConfigurationRepository(new Dictionary
                    <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                    {
                        {
                            key,
                            new Tuple
                                <string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                                    _fixture.Create<ConfigurationDataType>().ToString(),
                                    new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                                    {
                                        {
                                            versionInStore,
                                            new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                            {
                                                new Tuple<Guid, int, string, IEnumerable<string>>(
                                                    _fixture.Create<Guid>(),
                                                    _fixture.Create<int>(),
                                                    _fixture.Create<string>(),
                                                    _fixture.CreateMany<string>().ToList())
                                            }
                                        }
                                    })
                        }
                    });

            // act
            var config = await _sut
                .GetConfiguration(key, versionToQuery)
                .ConfigureAwait(false);

            // assert
            config.Should().BeNull();
        }

        public async Task GetConfiguration_ShouldReturnConfiguration_WhenItIsAvailable()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ConfigurationDataType>();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            _sut =
                new InMemoryConfigurationRepository(new Dictionary
                    <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                    {
                        {
                            key,
                            new Tuple
                                <string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                                    dataType.ToString(),
                                    new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                                    {
                                        {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                        {
                                            new Tuple<Guid, int, string, IEnumerable<string>>(
                                                valueId,
                                                sequence,
                                                data,
                                                tags)
                                        }}
                                    })
                        }
                    });

            var expectedConfig = new StoredConfig
            {
                Type = dataType.ToString(),
                Values = new List<StoredConfigValues>
                {
                    new StoredConfigValues
                    {
                        Id = valueId,
                        EnvironmentTags = tags,
                        Data = data,
                        Sequence = sequence
                    }
                }
            };

            // act
            var config = await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            // assert
            config.ShouldBeEquivalentTo(expectedConfig);
        }

        public async Task AddNewConfiguration_ShouldAddTheConfiguration_WhenItDoesNotExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ConfigurationDataType>().ToString();

            _sut = new InMemoryConfigurationRepository();

            var expected = new StoredConfig
            {
                Type = dataType,
                Values = new List<StoredConfigValues>()
            };

            // act
            await _sut
                .AddNewConfiguration(key, version, dataType)
                .ConfigureAwait(false);

            // assert
            (await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false))
                .ShouldBeEquivalentTo(expected);
        }

        public void AddNewConfiguration_ShouldThrowException_WhenItAlreadyExists()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ConfigurationDataType>();

            _sut =
                new InMemoryConfigurationRepository(new Dictionary
                    <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                    {
                        {
                            key,
                            new Tuple
                                <string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                                    dataType.ToString(),
                                    new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                                    {
                                        {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>()}
                                    })
                        }
                    });

            Func<Task> exThrower = async () => await _sut
                .AddNewConfiguration(key, version, dataType.ToString())
                .ConfigureAwait(false);

            // act/assert
            exThrower
                .ShouldThrow<ArgumentException>();
        }

        public async Task AddNewValueToConfiguration_ShouldAddTheNewValue_WhenConfigurationExists()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ConfigurationDataType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            _sut =
                new InMemoryConfigurationRepository(new Dictionary
                    <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                    {
                        {
                            key,
                            new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                                    dataType,
                                    new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                                    {
                                        {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>()}
                                    })
                        }
                    });

            var expected = new StoredConfig
            {
                Type = dataType,
                Values = new List<StoredConfigValues>
                {
                    new StoredConfigValues
                    {
                        Id = valueId,
                        Sequence = 1,
                        EnvironmentTags = tags,
                        Data = data
                    }
                }
            };

            // act
            await _sut
                .AddNewValueToConfiguration(key, version, valueId, tags, data)
                .ConfigureAwait(false);

            // assert
            (await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false))
                .ShouldBeEquivalentTo(expected);
        }

        public void AddNewValueToConfiguration_ShouldThrowException_WhenConfigurationDoesNotExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ConfigurationDataType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var unknownKey = _fixture.Create<string>();

            _sut =
                new InMemoryConfigurationRepository(new Dictionary
                    <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                    {
                        {
                            key,
                            new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                                    dataType,
                                    new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                                    {
                                        {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>()}
                                    })
                        }
                    });


            Func<Task> exceptionThrower = async () => await _sut
                .AddNewValueToConfiguration(unknownKey, version, valueId, tags, data)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower
                .ShouldThrow<ArgumentException>();
        }

        public async Task UpdateValueOnConfiguration_ShouldAddNewDataWithBumpedVersion_WhenConfigurationExists()
        {
            throw new NotImplementedException();
        }

        public void UpdateValueOnConfiguration_ShouldThrowException_WhenConfigurationDoesNotExist()
        {
            throw new NotImplementedException();
        }
    }
}