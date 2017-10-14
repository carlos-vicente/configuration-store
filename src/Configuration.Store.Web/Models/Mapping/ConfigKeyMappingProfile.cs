using AutoMapper;

namespace Configuration.Store.Web.Models.Mapping
{
    public class ConfigKeyMappingProfile : Profile
    {
        public ConfigKeyMappingProfile()
        {
            CreateMap<ConfigurationKey, ConfigKey>();
        }
    }
}