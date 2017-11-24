using System;
using System.Collections.Generic;

namespace Configuration.Store.Web.Contracts.Responses
{
    public class ConfigKeyDetailed
    {
        public string Key { get; set; }
        public Version LatestVersion { get; set; }
        public ValueType Type { get; set; }
        public IEnumerable<ConfigValueListItem> Values { get; set; }
    }
}