using System;
using System.Collections.Generic;

namespace Configuration.Store
{
    public class ConfigurationValue
    {
        public Guid Id { get; set; }
        public Version Version { get; set; }
        public string Data { get; set; }
        public int Sequence { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
