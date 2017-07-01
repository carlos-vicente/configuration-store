using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Configuration.Store.Persistence
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<StoredConfigKey>> GetConfigurations();
        Task<StoredConfig> GetConfiguration(string key, Version version);
        Task AddNewConfiguration(string key, Version version, string dataType);
        Task DeleteConfiguration(string key, Version version);
        Task AddNewValueToConfiguration(string key, Version version, Guid valueId, IEnumerable<string> envTags, string value);
        Task UpdateValueOnConfiguration(string key, Version version, Guid valueId, IEnumerable<string> envTags, string value);
        Task DeleteValueOnConfiguration(string key, Version version, Guid valueId);
    }
}
