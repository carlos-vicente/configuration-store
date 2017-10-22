using System;

namespace Configuration.Store.Web.Models
{
    public class ConfigKeyListItem : BaseModel
    {
        public string Key { get; set; }
        public string LatestVersion { get; set; }
        public ValueType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}