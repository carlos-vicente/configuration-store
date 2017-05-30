namespace Configuration.Store.Web.Contracts.Requests
{
    public class NewConfigurationRequest
    {
        public string Version { get; set; }
        public ConfigurationDataType Type { get; set; }
    }
}