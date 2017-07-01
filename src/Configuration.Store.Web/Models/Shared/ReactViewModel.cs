using Newtonsoft.Json;

namespace Configuration.Store.Web.Models.Shared
{
    public class ReactViewModel<T>
    {
        public string ComponentName { get; set; }
        public T Data { get; set; }
        public string DataJson
        {
            get
            {
                return JsonConvert.SerializeObject(Data);
            }
        }
        public string ContainerHtmlId { get; set; }
    }
}