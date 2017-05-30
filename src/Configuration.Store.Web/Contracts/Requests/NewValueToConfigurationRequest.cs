using System.Collections.Generic;

namespace Configuration.Store.Web.Contracts.Requests
{
    public class NewValueToConfigurationRequest
    {
        public IEnumerable<string> Tags { get; set; }
        public string Value { get; set; }
    }
}