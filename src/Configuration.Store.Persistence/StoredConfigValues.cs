using System;
using System.Collections.Generic;

namespace Configuration.Store.Persistence
{
    public class StoredConfigValues
    {
        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public string Data { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}