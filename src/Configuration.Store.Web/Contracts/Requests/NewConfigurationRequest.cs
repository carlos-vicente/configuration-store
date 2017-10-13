namespace Configuration.Store.Web.Contracts.Requests
{
    public class NewConfigurationRequest
    {
        public string Version { get; set; }
        public ValueType Type { get; set; }
    }
}