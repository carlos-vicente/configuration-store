using System;

namespace Configuration.Store
{
    public class Configuration
    {
        public int Sequence { get; set; }

        public ConfigurationDataType Type { get; set; }

        public string Data { get; set; }
    }
}