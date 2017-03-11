using System;
using System.Threading.Tasks;

namespace Configuration.Store
{
    public interface IConfigurationStoreService
    {
        Task<Configuration> GetConfiguration(
            string key,
            string version,
            int? currentSequence);

        Task<int> SetConfiguration(string key, string version, ConfigurationDataType dataType, string data);
    }
}
