﻿using AutoMapper;
using System.Linq;
using Configuration.Store.Web.Modules;

namespace Configuration.Store.Web.Contracts.Responses.Mapping
{
    public class ConfigKeyListMappingProfile : Profile
    {
        public ConfigKeyListMappingProfile()
        {
            CreateMap<ConfigurationKey, ConfigKeyListItem>()
                .ForMember(cfg => cfg.Links, cfg => cfg.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Links = new Nav[]
                    {
                        new Nav
                        {
                            Rel = "self",
                            Link = RouteRegistry.Ui.Configuration.GetConfigurationRoute(dest.Key.ToString())
                        }
                    };
                });
        }
    }
}