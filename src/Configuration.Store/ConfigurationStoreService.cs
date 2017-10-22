using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration.Store.Persistence;
using MassTransit;

namespace Configuration.Store
{
    public class ConfigurationStoreService : IConfigurationStoreService
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationStoreService(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ConfigurationKey>> GetConfigurationKeys()
        {
            return (await _repository
                .GetConfigurations()
                .ConfigureAwait(false))
                .Select(storedKey => new ConfigurationKey
                {
                    Key = storedKey.Key,
                    LatestVersion = storedKey.LastestVersion,
                    Type = (ValueType)Enum.Parse(typeof(ValueType), storedKey.Type),
                    CreatedAt = storedKey.CreatedAt
                })
                .ToList();
        }

        public async Task<Configuration> GetConfiguration(
            string key,
            Version version,
            string environmentTag,
            int? currentSequence)
        {
            //TODO: params check
            //TODO: check environment
            
            var config = await _repository
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            if (config == null)
                return null;


            var storedValue = config
                .Values
                .SingleOrDefault(val => val.EnvironmentTags.Contains(environmentTag));

            if (storedValue == null)
                return null;

            if (currentSequence.HasValue)
            {
                // check if there has been any change since last time it was obtained
                if (storedValue.Sequence == currentSequence.Value)
                    return new Configuration
                    {
                        Sequence = storedValue.Sequence,
                        Data = null
                    };
            }

            return new Configuration
            {
                Sequence = storedValue.Sequence,
                Data = storedValue.Data,
                Type = (ValueType)Enum.Parse(typeof(ValueType), config.Type)
            };
        }

        public async Task<ConfigurationKey> GetConfigurationKey(string key)
        {
            var versions = await _repository
                .GetConfigurationKeyVersions(key)
                .ConfigureAwait(false);

            if (versions == null)
                return null;

            var configKey = new ConfigurationKey
            {
                Key = key
            };

            var values = new List<ConfigurationValue>();
            foreach(var version in versions)
            {
                var configuration = await _repository
                    .GetConfiguration(key, version)
                    .ConfigureAwait(false);

                // TODO: no need to this multiple times, find a better concept representation/storage interface
                configKey.Type = (ValueType)Enum.Parse(typeof(ValueType), configuration.Type);

                var latestValue = configuration
                    .Values
                    .OrderByDescending(val => val.Sequence)
                    .FirstOrDefault();

                if (latestValue == null)
                    continue;

                values.Add(new ConfigurationValue
                {
                    Version = version,
                    LatestData = latestValue.Data,
                    LatestSequence = latestValue.Sequence,
                    EnvironmentTags = latestValue.EnvironmentTags,
                    CreatedAt = latestValue.CreatedAt
                });
            }

            configKey.Values = values
                .OrderBy(val => val.Version)
                .ToArray();

            return configKey;
        }

        public async Task AddConfiguration(
            string key,
            Version version,
            ValueType dataType)
        {
            // todo: params check

            var currentConfig = await _repository
                .GetConfiguration(key, version)
                .ConfigureAwait(false);

            if(currentConfig != null)
                throw new ArgumentException($"Can not create configuration with key {key} and version {version}, as it already exists!");

            await _repository
                .AddNewConfiguration(key, version, dataType.ToString(), DateTime.UtcNow)
                .ConfigureAwait(false);
        }

        public async Task RemoveConfiguration(
            string key,
            Version version)
        {
            var currentConfig = await _repository
              .GetConfiguration(key, version)
              .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not delete value on configuration with key {key} and version {version}, as it does not exists!");

            await _repository
                .DeleteConfiguration(key, version)
                .ConfigureAwait(false);
        }

        public async Task<Guid> AddValueToConfiguration(
            string key,
            Version version,
            IEnumerable<string> tags,
            string value)
        {
            var currentConfig = await _repository
               .GetConfiguration(key, version)
               .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not add value to configuration with key {key} and version {version}, as it does not exists!");

            // check there are no tag overlaps between diferent values from same key and version
            var storedValue = currentConfig
                .Values
                .FirstOrDefault(val => val.EnvironmentTags.Any(tags.Contains));
            if (storedValue != null)
                throw new ArgumentException($"There is already a configuration value for environment tags {string.Join(";", storedValue.EnvironmentTags)} which match at least one of {string.Join(";", tags)}");

            var valueId = NewId.NextGuid();

            // there is no overlap, so add it
            await _repository
                .AddNewValueToConfiguration(
                    key,
                    version,
                    valueId,
                    tags,
                    value,
                    DateTime.UtcNow)
                .ConfigureAwait(false);

            return valueId;
        }

        public async Task UpdateValueOnConfiguration(
            string key,
            Version version,
            Guid valueId,
            IEnumerable<string> tags,
            string value)
        {
            var currentConfig = await _repository
               .GetConfiguration(key, version)
               .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not update value on configuration with key {key} and version {version}, as it does not exists!");

            var storedValue = currentConfig
                .Values
                .SingleOrDefault(val => val.Id == valueId);

            if(storedValue == null)
                throw new ArgumentException($"There is no configuration value for key {key} on version {version} with identifier {valueId}");

            await _repository
                .UpdateValueOnConfiguration(key, version, valueId, tags, value)
                .ConfigureAwait(false);
        }

        public async Task RemoveValueFromConfiguration(
            string key,
            Version version,
            Guid valueId)
        {
            var currentConfig = await _repository
               .GetConfiguration(key, version)
               .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not delete value on configuration with key {key} and version {version}, as it does not exists!");

            var storedValue = currentConfig
                .Values
                .SingleOrDefault(val => val.Id == valueId);

            if (storedValue == null)
                throw new ArgumentException($"There is no configuration value for key {key} on version {version} with identifier {valueId}");

            await _repository
                .DeleteValueOnConfiguration(key, version, valueId)
                .ConfigureAwait(false);
        }
    }
}