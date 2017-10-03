using Nancy.ViewEngines.Razor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Configuration.Store.Web.ViewHelpers
{
    public static class HtmlJsonHelper
    {
        private static JsonSerializerSettings Settings = new JsonSerializerSettings
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
                new StringEnumConverter()
            }
        };

        public static IHtmlString AsJson<T>(this HtmlHelpers<T> helpers, object obj)
        {
            return new NonEncodedHtmlString(JsonConvert.SerializeObject(obj, Settings));
        }
    }
}