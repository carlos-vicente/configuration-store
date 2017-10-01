﻿using System;

namespace Configuration.Store
{
    public class ConfigurationKey
    {
        public string Key { get; set; }
        public Version LatestVersion { get; set; }
        public ConfigurationDataType Type { get; set; }
    }
}