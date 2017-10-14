using Configuration.Store.Web.Models;
using Nancy;
using System;

namespace Configuration.Store.Web.Modules.Ui
{
    public class SwaggerNancyModule : NancyModule
    {
        public SwaggerNancyModule()
        {
            Get["/swagger"] = _ => View["swagger-ui", new SwaggerViewModel
            {
                ApiDocsPath = new Uri(new Uri(Request.Url.SiteBase), "/api-docs")
            }];
        }
    }
}