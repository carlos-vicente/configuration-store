using AutoMapper;
using Microsoft.Owin.Extensions;
using Owin;
using System.Reflection;

namespace Configuration.Store.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);

            Mapper.Initialize(config => config.AddProfiles(Assembly.GetExecutingAssembly()));
        }
    }
}