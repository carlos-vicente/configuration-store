using System;
using System.Collections.Generic;

namespace Configuration.Store
{
    public class ConfigurationKey
    {
        public string Key { get; set; }
        public Version LatestVersion { get; set; }
        public ValueType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<ConfigurationValue> Values { get; set; }
    }
}
