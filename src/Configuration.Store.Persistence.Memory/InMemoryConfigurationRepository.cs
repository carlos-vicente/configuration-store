using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Store.Persistence.Memory
{
    public class InMemoryConfigurationRepository : IConfigurationRepository
    {
        //[key][version]{data-type, sequence, data}
        private IDictionary<string, IDictionary<string, Tuple<string, int, string>>> configs;

        public InMemoryConfigurationRepository()
        {
            configs = new ConcurrentDictionary<string, IDictionary<string, Tuple<string, int, string>>>();
        }

        public Task<int> GetSequence(string key, string version)
        {
            var sequence = 0;
            if (configs.ContainsKey(key))
            {
                if (configs[key].ContainsKey(version))
                {
                    sequence = configs[key][version].Item2;
                }
            }

            return Task.FromResult(sequence);
        }

        public Task<StoredConfig> GetConfiguration(string key, string version)
        {
            StoredConfig config = null;

            if (configs.ContainsKey(key))
            {
                if (configs[key].ContainsKey(version))
                {
                    config = new StoredConfig
                    {
                        Type = configs[key][version].Item1,
                        Data = configs[key][version].Item3
                    };
                }
            }

            return Task.FromResult(config);
        }

        public Task SaveNewConfiguration(
            string key,
            string version,
            int sequence,
            string dataType,
            string data)
        {
            throw new System.NotImplementedException();
        }
    }
}