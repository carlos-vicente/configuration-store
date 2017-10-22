using System;
using System.Collections.Generic;

namespace Configuration.Store.Web.Models
{
    public class ConfigValueListItem
    {
        public string Version { get; set; }
        public string LatestData { get; set; }
        public int LatestSequence { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}