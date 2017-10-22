using System.Collections.Generic;

namespace Configuration.Store.Web.Models
{
    public class ConfigKeyDetailed
    {
        public string Key { get; set; }
        public ValueType Type { get; set; }
        public IEnumerable<ConfigValueListItem> Values { get; set; }
    }
}