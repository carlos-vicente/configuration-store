namespace Configuration.Store.Web.Modules
{
    public static class RouteRegistry
    {
        public static class Api
        {
            public static class Configuration
            {
                public static readonly string GetConfigForVersion = "/api/{configKey}/{configVersion:version}/{envTag}";
                public static readonly string AddNewConfiguration = "/api/{configKey}";
                public static readonly string AddNewValueToConfiguration = "/api/{configKey}/{configVersion:version}/values";
                public static readonly string UpdateValueOnConfiguration = "/api/{configKey}/{configVersion:version}/values/{valueId:guid}";
                public static readonly string DeleteValueFromConfiguration = "/api/{configKey}/{configVersion:version}/values/{valueId:guid}";
                public static readonly string DeleteConfiguration = "/api/{configKey}/{configVersion:version}";
            }
        }
    }
}