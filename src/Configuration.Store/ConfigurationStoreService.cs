using System;
using System.Threading.Tasks;
using Configuration.Store.Persistence;

namespace Configuration.Store
{
    public class ConfigurationStoreService : IConfigurationStoreService
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationStoreService(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Configuration> GetConfiguration(
            string key,
            string version,
            int? currentSequence)
        {
            //TODO: params check

            var sequence = await _repository
                .GetSequence(key, version)
                .ConfigureAwait(false);

            if (currentSequence.HasValue)
            {
                // check if there has been any change since last time it was obtained
                if (sequence > currentSequence.Value)
                    return new Configuration
                    {
                        Sequence = sequence,
                        Data = null
                    };
            }

            var config = await _repository
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            if(config == null)
                return null;

            return new Configuration
            {
                Sequence = sequence,
                Data = config.Data,
                Type = (ConfigurationDataType)Enum.Parse(typeof(ConfigurationDataType), config.Type)
            };
        }

        public async Task<int> SetConfiguration(
            string key,
            string version,
            ConfigurationDataType dataType,
            string data)
        {
            //TODO: params check

            // check if there is already a configuration for this {key, version}
            var currentSequence = await _repository
                .GetSequence(key, version)
                .ConfigureAwait(false);

            var newSequence = currentSequence + 1;

            await _repository.SaveNewConfiguration(
                    key,
                    version,
                    newSequence,
                    dataType.ToString(),
                    data)
                .ConfigureAwait(false);

            return newSequence;
        }
    }
}