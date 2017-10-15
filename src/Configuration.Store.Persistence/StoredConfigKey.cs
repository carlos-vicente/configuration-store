using System;

namespace Configuration.Store.Persistence
{
    public class StoredConfigKey
    {
        public string Key { get; set; }
        public Version LastestVersion { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
