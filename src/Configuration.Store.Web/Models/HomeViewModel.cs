using System.Collections.Generic;

namespace Configuration.Store.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ConfigKey> ConfigKeys { get; set; }
    }
}