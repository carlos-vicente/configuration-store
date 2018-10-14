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
                    Type = (ValueType)Enum.Parse(typeof(ValueType), storedKey.Type),
                    CreatedAt = storedKey.CreatedAt
                })
                .OrderBy(k => k.Key)
                .ToList();
        }

        public async Task<ConfigurationKey> GetConfigurationKey(string key)
        {
            var storedConfig = await _repository
                .GetConfiguration(key)
                .ConfigureAwait(false);

            var configKey = new ConfigurationKey
            {
                Key = key,
                Type = (ValueType)Enum.Parse(typeof(ValueType), storedConfig.Type),
                Values = storedConfig
                    .Values
                    .Select(storedValue => new ConfigurationValue
                    {
                        Id = storedValue.Id,
                        Sequence = storedValue.Sequence,
                        CreatedAt = storedValue.CreatedAt,
                        Version = storedValue.Version,
                        Data = storedValue.Data,
                        EnvironmentTags = storedValue.EnvironmentTags
                    })
            };

            return configKey;
        }

        public async Task<ConfigurationValue> GetConfigurationValue(
            string key,
            Version version,
            string environmentTag)
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

            return new ConfigurationValue
            {
                Id = storedValue.Id,
                Version = version,
                Sequence = storedValue.Sequence,
                Data = storedValue.Data,
                EnvironmentTags = storedValue.EnvironmentTags,
                CreatedAt = storedValue.CreatedAt
            };
        }

        public async Task AddConfiguration(
            string key,
            ValueType dataType)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Provide a valid key name");
            if (!Enum.IsDefined(typeof(ValueType), dataType))
                throw new ArgumentException("Provide a valid key value type");

            var currentConfig = await _repository
                .GetConfiguration(key)
                .ConfigureAwait(false);

            if(currentConfig != null)
                throw new ArgumentException($"Can not create configuration with key {key}, as it already exists!");

            await _repository
                .AddNewConfiguration(key, dataType.ToString(), DateTime.UtcNow)
                .ConfigureAwait(false);
        }

        public async Task RemoveConfiguration(string key)
        {
            var currentConfig = await _repository
              .GetConfiguration(key)
              .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not delete value on configuration with key {key}, as it does not exists!");

            await _repository
                .DeleteConfiguration(key)
                .ConfigureAwait(false);
        }

        public async Task RemoveConfigurationVersion(
            string key,
            Version version)
        {
            var currentConfig = await _repository
              .GetConfiguration(key, version)
              .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not delete value on configuration with key {key} and version {version}, as it does not exists!");

            await _repository
                .DeleteConfigurationVersion(key, version)
                .ConfigureAwait(false);
        }

        public async Task<Guid> AddValueToConfiguration(
            string key,
            Version version,
            IEnumerable<string> tags,
            string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Provide a valid key name");
            if (version == default(Version))
                throw new ArgumentException("Provide a valid version");
            if (tags == null || !tags.Any())
                throw new ArgumentException("Provide a valid collection of environment tags");

            // when value type String: value can be anything, even empty string, it's up to the client app to deal with it
            // TODO when value type JSON: make sure it is a valid JSON

            var currentConfig = await _repository
               .GetConfiguration(key)
               .ConfigureAwait(false);

            if (currentConfig == null)
                throw new ArgumentException($"Can not add value to configuration with key {key}, as it does not exists!");

            // check there are no tag overlaps between diferent values from same key and version
            var storedValue = currentConfig
                .Values
                .FirstOrDefault(val => val.Version == version && val.EnvironmentTags.Any(tags.Contains));
            
            if (storedValue != null)
                throw new ArgumentException($"There is already a configuration value for environment tags {string.Join(";", storedValue.EnvironmentTags)} which match at least one of {string.Join(";", tags)} for version {version}");

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