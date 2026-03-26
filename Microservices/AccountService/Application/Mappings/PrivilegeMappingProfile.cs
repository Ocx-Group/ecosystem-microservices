using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Privilege;
using Ecosystem.AccountService.Application.DTOs.Privilege;
using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Application.Mappings;

public class PrivilegeMappingProfile : Profile
{
    public PrivilegeMappingProfile()
    {
        CreateMap<Domain.Models.Privilege, PrivilegesDto>();
        CreateMap<CreatePrivilegeCommand, Domain.Models.Privilege>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
        CreateMap<MenuConfiguration, PrivilegeMenuConfigurationDto>()
            .ForMember(d => d.MenuConfigurationId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.PrivilegeId, opt => opt.Ignore())
            .ForMember(d => d.CanCreate, opt => opt.Ignore())
            .ForMember(d => d.CanRead, opt => opt.Ignore())
            .ForMember(d => d.CanDelete, opt => opt.Ignore())
            .ForMember(d => d.CanEdit, opt => opt.Ignore());
    }
}
