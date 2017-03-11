using System;
using System.Threading.Tasks;

namespace Configuration.Store.Persistence
{
    public interface IConfigurationRepository
    {
        Task<int> GetSequence(string key, string version);
        Task<string> GetConfiguration(string key, string version);
        Task SaveNewConfiguration(string key, string version, int sequence, string data);
    }
}
