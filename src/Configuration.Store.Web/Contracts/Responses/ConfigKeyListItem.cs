using System;

namespace Configuration.Store.Web.Contracts.Responses
{
    public class ConfigKeyListItem : BaseModel
    {
        public string Key { get; set; }
        public ValueType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}