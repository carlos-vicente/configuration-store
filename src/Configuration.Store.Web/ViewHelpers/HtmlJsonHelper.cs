using Nancy.ViewEngines.Razor;
using Newtonsoft.Json;

namespace Configuration.Store.Web.ViewHelpers
{
    public static class HtmlJsonHelper
    {
        private static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            StringEscapeHandling = StringEscapeHandling.EscapeHtml
        };

        public static IHtmlString AsJson<T>(this HtmlHelpers<T> helpers, object obj)
        {
            return new NonEncodedHtmlString(JsonConvert.SerializeObject(obj, Settings));
        }
    }
}