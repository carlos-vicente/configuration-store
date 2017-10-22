using AutoMapper;

namespace Configuration.Store.Web.Models.Mapping
{
    public class ConfigKeyDetailMappingProfile: Profile
    {
        public ConfigKeyDetailMappingProfile()
        {
            CreateMap<ConfigurationValue, ConfigValueListItem>();
            CreateMap<ConfigurationKey, ConfigKeyDetailed>();
        }
    }
}