using System.Collections.Generic;

namespace Configuration.Store.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ConfigKey> ConfigKeys { get; set; }
    }

    public class ConfigKey
    {
        public string Key { get; set; }
        public string LatestVersion { get; set; }
        public ValueType Type { get; set; }
    }
}