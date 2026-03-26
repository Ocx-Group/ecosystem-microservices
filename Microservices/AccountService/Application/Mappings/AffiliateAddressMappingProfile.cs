using AutoMapper;
using Ecosystem.AccountService.Application.Commands.AffiliateAddress;
using Ecosystem.AccountService.Application.DTOs.AffiliateAddress;
using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Application.Mappings;

public class AffiliateAddressMappingProfile : Profile
{
    public AffiliateAddressMappingProfile()
    {
        CreateMap<AffiliatesAddress, AffiliateAddressDto>();
        CreateMap<CreateAffiliateAddressCommand, AffiliatesAddress>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
    }
}
