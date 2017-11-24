using System;
using System.Collections.Generic;

namespace Configuration.Store.Web.Contracts.Responses
{
    public class ConfigValueListItem
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string Data { get; set; }
        public int Sequence { get; set; }
        public IEnumerable<string> EnvironmentTags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}