using AutoMapper;
using Configuration.Store.Web.Models;
using Microsoft.Owin.Extensions;
using Owin;

namespace Configuration.Store.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);

            Mapper.Initialize(config => config.AddProfile<ConfigKeyMappingProfile>());
        }
    }
}