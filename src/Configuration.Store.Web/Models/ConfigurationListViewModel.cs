using System.Collections.Generic;

namespace Configuration.Store.Web.Models
{
    public class ConfigurationListViewModel
    {
        public IEnumerable<ConfigKeyListItem> ConfigKeys { get; set; }
    }
}