using System.Threading.Tasks;

namespace Configuration.Store.Persistence
{
    public class StoredConfig
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }

    public interface IConfigurationRepository
    {
        Task<int> GetSequence(string key, string version);
        Task<StoredConfig> GetConfiguration(string key, string version);
        Task SaveNewConfiguration(string key, string version, int sequence, string dataType, string data);
    }
}
