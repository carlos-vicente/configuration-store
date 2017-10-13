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
                                    _fixture.Create<ValueType>().ToString(),
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
            var dataType = _fixture.Create<ValueType>();
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
            var dataType = _fixture.Create<ValueType>().ToString();

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
            var dataType = _fixture.Create<ValueType>();

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
            var dataType = _fixture.Create<ValueType>().ToString();
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
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
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
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var newData = _fixture.Create<string>();

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
                                        {
                                            version,
                                            new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                            {
                                                new Tuple<Guid, int, string, IEnumerable<string>>(
                                                    valueId,
                                                    sequence,
                                                    data,
                                                    tags)
                                            }
                                        }
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
                        Sequence = sequence + 1,
                        EnvironmentTags = tags,
                        Data = newData
                    }
                }
            };

            // act
            await _sut
                .UpdateValueOnConfiguration(key, version, valueId, tags, newData)
                .ConfigureAwait(false);

            // actual
            (await _sut
                .GetConfiguration(key, version)
                .ConfigureAwait(false))
                .ShouldBeEquivalentTo(expected);
        }

        public void UpdateValueOnConfiguration_ShouldThrowException_WhenConfigurationDoesNotExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
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
                                    {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            1,
                                            data,
                                            tags)
                                    }}
                                })
                        }
                    });


            Func<Task> exceptionThrower = async () => await _sut
                .UpdateValueOnConfiguration(unknownKey, version, valueId, tags, data)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower
                .ShouldThrow<ArgumentException>();
        }

        public void UpdateValueOnConfiguration_ShouldThrowException_WhenConfigurationExistsButNotTheVersion()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var tags = _fixture.CreateMany<string>().ToList();

            var unknownVersion = _fixture.Create<Version>();

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
                                    {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            1,
                                            data,
                                            tags)
                                    }}
                                })
                        }
                    });


            Func<Task> exceptionThrower = async () => await _sut
                .UpdateValueOnConfiguration(key, unknownVersion, valueId, tags, data)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower
                .ShouldThrow<ArgumentException>();
        }

        public void UpdateValueOnConfiguration_ShouldThrowException_WhenConfigurationExistsWithVersionButNoValueIdentifier()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var tags = _fixture.CreateMany<string>().ToList();

            var unknownValueId = _fixture.Create<Guid>();

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
                                    {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            1,
                                            data,
                                            tags)
                                    }}
                                })
                        }
                    });

            Func<Task> exceptionThrower = async () => await _sut
                .UpdateValueOnConfiguration(key, version, unknownValueId, tags, data)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower
                .ShouldThrow<ArgumentException>();
        }

        public async Task DeleteConfiguration_ShouldDeleteConfiguration_WhenConfigurationExists()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>()
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            // act
            await _sut
                .DeleteConfiguration(key, version)
                .ConfigureAwait(false);

            // assert
            store[key]
                .Item2
                .ContainsKey(version)
                .Should()
                .BeFalse();
        }

        public void DeleteConfiguration_ShouldThrowException_WhenConfigurationDoesNotExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();

            var wrongKey = _fixture.Create<string>();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>()
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            Func<Task> exceptionThrower = async () => await _sut
                .DeleteConfiguration(wrongKey, version)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower.ShouldThrow<ArgumentException>();
        }

        public void DeleteConfiguration_ShouldThrowException_WhenConfigurationExistsButVersionDoesNot()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();

            var wrongVersion = _fixture.Create<Version>();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>()
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            Func<Task> exceptionThrower = async () => await _sut
                .DeleteConfiguration(key, wrongVersion)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower.ShouldThrow<ArgumentException>();
        }

        public async Task DeleteValueOnConfiguration_ShouldDeleteValue_WhenConfigurationAndValueExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            sequence,
                                            data,
                                            tags)
                                    }
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            // act
            await _sut
                .DeleteValueOnConfiguration(key, version, valueId)
                .ConfigureAwait(false);

            // assert
            store[key]
                .Item2[version]
                .FirstOrDefault(t => t.Item1 == valueId)
                .Should()
                .BeNull();
        }

        public void DeleteValueOnConfiguration_ShouldThrowException_WhenConfigurationDoesNotExist()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var wrongKey = _fixture.Create<string>();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            sequence,
                                            data,
                                            tags)
                                    }
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            Func<Task> exceptionThrower = async () => await _sut
                .DeleteValueOnConfiguration(wrongKey, version, valueId)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower.ShouldThrow<ArgumentException>();
        }

        public void DeleteValueOnConfiguration_ShouldThrowException_WhenConfigurationExistsButVersionDoesNot()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var wrongVersion = _fixture.Create<Version>();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            sequence,
                                            data,
                                            tags)
                                    }
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            Func<Task> exceptionThrower = async () => await _sut
                .DeleteValueOnConfiguration(key, wrongVersion, valueId)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower.ShouldThrow<ArgumentException>();
        }

        public void DeleteValueOnConfiguration_ShouldThrowException_WhenConfigurationAndVersionExistButValueDoesNot()
        {
            // arrange
            var key = _fixture.Create<string>();
            var version = _fixture.Create<Version>();
            var dataType = _fixture.Create<ValueType>().ToString();
            var valueId = _fixture.Create<Guid>();
            var data = _fixture.Create<string>();
            var sequence = _fixture.Create<int>();
            var tags = _fixture.CreateMany<string>().ToList();

            var wrongValueId = _fixture.Create<Guid>();

            var store = new Dictionary
                <string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>
                {
                    {
                        key,
                        new Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                            dataType,
                            new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                            {
                                {
                                    version,
                                    new List<Tuple<Guid, int, string, IEnumerable<string>>>
                                    {
                                        new Tuple<Guid, int, string, IEnumerable<string>>(
                                            valueId,
                                            sequence,
                                            data,
                                            tags)
                                    }
                                }
                            })
                    }
                };

            _sut = new InMemoryConfigurationRepository(store);

            Func<Task> exceptionThrower = async () => await _sut
                .DeleteValueOnConfiguration(key, version, wrongValueId)
                .ConfigureAwait(false);

            // act/assert
            exceptionThrower.ShouldThrow<ArgumentException>();
        }
    }
}