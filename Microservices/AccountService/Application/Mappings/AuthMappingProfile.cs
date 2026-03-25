using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Commands.Auth;
using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UsersAffiliate, UsersAffiliatesDto>()
            .ForMember(d => d.CountryNavigation, opt => opt.MapFrom(s => s.CountryNavigation));
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