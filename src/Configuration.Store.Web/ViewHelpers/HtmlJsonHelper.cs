using Configuration.Store.Web.Serialization;
using Nancy.ViewEngines.Razor;
using Newtonsoft.Json;

namespace Configuration.Store.Web.ViewHelpers
{
    public static class HtmlJsonHelper
    {
        public static IHtmlString AsJson<T>(this HtmlHelpers<T> helpers, object obj)
        {
            var serialized = JsonConvert.SerializeObject(obj, JsonSerializationOptions.Settings);
            return new NonEncodedHtmlString(serialized);
        }
    }
}