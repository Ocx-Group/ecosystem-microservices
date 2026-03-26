using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.MenuConfiguration;

namespace Ecosystem.AccountService.Application.Mappings;

public class MenuConfigurationMappingProfile : Profile
{
    public MenuConfigurationMappingProfile()
    {
        CreateMap<Domain.Models.MenuConfiguration, MenuConfigurationDto>();
    }
}
