using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Configuration.Store.Web.Serialization
{
    public static class JsonSerializationOptions
    {
        public static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter
                {
                    AllowIntegerValues = false,
                    CamelCaseText = false
                }
            }
        };
    }
}