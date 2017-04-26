﻿using System;
using System.Collections.Generic;

namespace Configuration.Store.Persistence
{
    public class StoredConfig
    {
        public string Type { get; set; }
        public IEnumerable<StoredConfigValues> Values { get; set; }
    }

    public class StoredConfigValues
    {
        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public string Data { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
    }
}