namespace Configuration.Store.Web.Modules
{
    public static class RouteRegistry
    {
        public class Route
        {
            public string Name { get; set; }
            public string Template { get; set; }
        }

        public static class Api
        {
            public static class Configuration
            {
                public static readonly Route GetConfigs = new Route
                {
                    Name = "GetConfigs",
                    Template = "/api/keys"
                };
                public static readonly Route GetConfigForVersion = new Route
                {
                    Name = "GetConfigForVersion",
                    Template = "/api/keys/{configKey}/version/{configVersion:version}/{envTag}"
                };
                public static readonly Route AddNewConfiguration = new Route
                {
                    Name = "AddNewConfiguration",
                    Template = "/api/keys/{configKey}"
                };
                public static readonly Route AddNewValueToConfiguration = new Route
                {
                    Name = "AddNewValueToConfiguration",
                    Template = "/api/keys/{configKey}/version/{configVersion:version}/values"
                };
                public static readonly Route UpdateValueOnConfiguration = new Route
                {
                    Name = "UpdateValueOnConfiguration",
                    Template = "/api/keys/{configKey}/version/{configVersion:version}/values/{valueId:guid}"
                };
                public static readonly Route DeleteValueFromConfiguration = new Route
                {
                    Name = "DeleteValueFromConfiguration",
                    Template = "/api/keys/{configKey}/version/{configVersion:version}/values/{valueId:guid}"
                };
                public static readonly Route DeleteConfiguration = new Route
                {
                    Name = "DeleteConfiguration",
                    Template = "/api/keys/{configKey}/version/{configVersion:version}"
                };
            }
        }

        public static class Ui
        {
            public static class Configuration
            {
                public static readonly string GetHome = "/";
                public static readonly string GetConfiguration = "/key/{configKey}";

                public static string GetConfigurationRoute(string configKey)
                {
                    return GetConfiguration.Replace("{configKey}", configKey);
                }
            }
        }
    }
}