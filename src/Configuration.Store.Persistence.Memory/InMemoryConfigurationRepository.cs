using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configuration.Store.Persistence.Memory
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        //[key]{data-type, [version]{id, sequence, data, tags*}*}
        private readonly IDictionary<string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>> _configs;

        // JUST FOR TESTING, NEVER MAKE IT PUBLIC
        internal InMemoryConfigurationRepository(
            IDictionary<string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>> configs)
        {
            _configs = configs;
        }

        public InMemoryConfigurationRepository()
        {
            _configs = new ConcurrentDictionary<string, Tuple<string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>>();
        }

        public Task<IEnumerable<StoredConfigKey>> GetConfigurations()
        {
            return Task.FromResult(_configs
                .Keys
                .Select(key =>
                {
                    var latestVersion = _configs[key]
                        .Item2
                        .Keys
                        .OrderByDescending(k => k)
                        .First();
                    return new StoredConfigKey
                    {
                        Key = key,
                        Type = _configs[key].Item1,
                        LastestVersion = latestVersion,
                        CreatedAt = DateTime.UtcNow // this is a memory stub, really don't care about when it was created
                    };
                }));
        }

        public Task<StoredConfig> GetConfiguration(string key, Version version)
        {
            StoredConfig config = null;

            if (_configs.ContainsKey(key))
            {
                if (_configs[key].Item2.ContainsKey(version))
                {
                    config = new StoredConfig
                    {
                        Type = _configs[key].Item1,
                        Values = _configs[key]
                            .Item2[version]
                            .Select(tuple => new StoredConfigValues
                            {
                                Id = tuple.Item1,
                                Sequence = tuple.Item2,
                                Data = tuple.Item3,
                                EnvironmentTags = tuple.Item4,
                                CreatedAt = DateTime.UtcNow // this is a memory stub, really don't care about when it was created
                            })
                    };
                }
            }

            return Task.FromResult(config);
        }

        public Task AddNewConfiguration(
            string key,
            Version version,
            string dataType,
            DateTime createdAt)
        {
            if (_configs.ContainsKey(key))
                throw new ArgumentException("key already available");

            _configs[key] = new Tuple
                <string, IDictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>>(
                    dataType, new Dictionary<Version, IList<Tuple<Guid, int, string, IEnumerable<string>>>>
                    {
                        {version, new List<Tuple<Guid, int, string, IEnumerable<string>>>()}
                    });

            return Task.FromResult(0);
        }

        public Task AddNewValueToConfiguration(
            string key,
            Version version,
            Guid valueId,
            IEnumerable<string> envTags,
            string value,
            DateTime createdAt)
        {
            if (!_configs.ContainsKey(key) || !_configs[key].Item2.ContainsKey(version))
                throw new ArgumentException($"key {key} for version {version} not found");

            var valueToStore = new Tuple<Guid, int, string, IEnumerable<string>>(
                valueId,
                1,
                value,
                envTags);

            _configs[key].Item2[version].Add(valueToStore);

            return Task.FromResult(0);
        }

        public Task UpdateValueOnConfiguration(
            string key,
            Version version,
            Guid valueId,
            IEnumerable<string> envTags,
            string value)
        {
            if (!_configs.ContainsKey(key) 
                || !_configs[key].Item2.ContainsKey(version)
                || _configs[key].Item2[version].All(t => t.Item1 != valueId))
                throw new ArgumentException($"key {key} for version {version} not found");

            var config = _configs[key].Item2[version]
                .SingleOrDefault(val => val.Item1 == valueId);

            var newConfig = new Tuple<Guid, int, string, IEnumerable<string>>(
                valueId,
                config.Item2 + 1,
                value,
                envTags);

            var index = _configs[key].Item2[version].IndexOf(config);

            _configs[key].Item2[version][index] = newConfig;

            return Task.FromResult(0);
        }

        public Task DeleteConfiguration(string key, Version version)
        {
            if(!_configs.ContainsKey(key)
                || !_configs[key].Item2.ContainsKey(version))
                throw new ArgumentException($"key {key} for version {version} not found");

            _configs[key].Item2.Remove(version);

            return Task.FromResult(0);
        }

        public Task DeleteValueOnConfiguration(string key, Version version, Guid valueId)
        {
            if (!_configs.ContainsKey(key)
                || !_configs[key].Item2.ContainsKey(version)
                || _configs[key].Item2[version].All(t => t.Item1 != valueId))
                throw new ArgumentException($"key {key} for version {version} not found");

            var config = _configs[key]
                .Item2[version]
                .SingleOrDefault(val => val.Item1 == valueId);

            _configs[key].Item2[version].Remove(config);

            return Task.FromResult(0);
        }
    }
}