using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Role;
using Ecosystem.AccountService.Application.DTOs.Role;

namespace Ecosystem.AccountService.Application.Mappings;

public class RoleMappingProfile : Profile
{
    public RoleMappingProfile()
    {
        CreateMap<Domain.Models.Role, RoleDto>();
        CreateMap<CreateRoleCommand, Domain.Models.Role>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));
    }
}
