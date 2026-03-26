using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Application.Mappings;

public class AffiliateBtcMappingProfile : Profile
{
    public AffiliateBtcMappingProfile()
    {
        CreateMap<AffiliatesBtc, AffiliateBtcDto>();
    }
}
