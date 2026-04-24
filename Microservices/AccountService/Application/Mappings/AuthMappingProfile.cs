using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Commands.Auth;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.Domain.Core.Storage;

namespace Ecosystem.AccountService.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.RolName, opt => opt.MapFrom(s => s.Rol != null ? s.Rol.Name : string.Empty))
            .ForMember(d => d.ImageProfileUrl, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.ImageProfileUrl) ?? string.Empty));
        CreateMap<UsersAffiliate, UsersAffiliatesDto>()
            .ForMember(d => d.CountryNavigation, opt => opt.MapFrom(s => s.CountryNavigation))
            .ForMember(d => d.ImageProfileUrl, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.ImageProfileUrl)))
            .ForMember(d => d.ImageIdPath, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.ImagePathId)));
        CreateMap<Country, CountryDto>();
        CreateMap<LoginMovement, LoginMovementsDto>();
        CreateMap<UserAuthenticationCommand, LoginMovement>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.BrowserInfo, opt => opt.MapFrom(s => s.BrowserInfo))
            .ForMember(d => d.OperatingSystem, opt => opt.MapFrom(s => s.OperatingSystem))
            .ForMember(d => d.IpAddress, opt => opt.MapFrom(s => s.IpAddress))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => true))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(s => DateTime.Now))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(s => DateTime.Now));
    }
}
