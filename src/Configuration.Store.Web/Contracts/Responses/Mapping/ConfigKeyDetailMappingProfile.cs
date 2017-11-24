using AutoMapper;

namespace Configuration.Store.Web.Contracts.Responses.Mapping
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