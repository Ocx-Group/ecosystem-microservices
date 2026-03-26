using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Application.Mappings;

public class AffiliateInfoMappingProfile : Profile
{
    public AffiliateInfoMappingProfile()
    {
        CreateMap<UsersAffiliate, UserEcoPoolDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.Username));

        CreateMap<CountryNetwork, CountryNetworkDto>();
    }
}
