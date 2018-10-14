using Configuration.Store.Web.Contracts.Responses;
using System.Collections.Generic;

namespace Configuration.Store.Web.Views.Models
{
    public class ConfigurationListViewModel
    {
        public IEnumerable<ConfigKeyListItem> ConfigKeys { get; set; }
    }
}