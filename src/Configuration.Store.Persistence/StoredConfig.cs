using System.Collections.Generic;

namespace Configuration.Store.Persistence
{
    public class StoredConfig
    {
        public string Type { get; set; }
        public IEnumerable<StoredConfigValues> Values { get; set; }
    }
}