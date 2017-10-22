using System;
using System.Collections.Generic;

namespace Configuration.Store
{
    public class ConfigurationValue
    {
        public Version Version { get; set; }
        public string LatestData { get; set; }
        public int LatestSequence { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
