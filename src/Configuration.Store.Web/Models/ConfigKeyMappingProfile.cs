using AutoMapper;

namespace Configuration.Store.Web.Models
{
    public class ConfigKeyMappingProfile : Profile
    {
        public ConfigKeyMappingProfile()
        {
            CreateMap<ConfigurationKey, ConfigKey>();
        }
    }
}