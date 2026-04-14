using System.Globalization;
using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Ecosystem.Grpc.Account;

namespace Ecosystem.AccountService.Api.Mappings;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<UsersAffiliatesDto, UserInfoMessage>(MemberList.None)
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name ?? string.Empty))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.LastName ?? string.Empty))
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Phone ?? string.Empty))
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Address ?? string.Empty))
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.City ?? string.Empty))
            .ForMember(d => d.AffiliateType, opt => opt.MapFrom(s => s.AffiliateType ?? string.Empty))
            .ForMember(d => d.ActivationDate,
                opt => opt.MapFrom(s =>
                    s.ActivationDate.HasValue ? s.ActivationDate.Value.ToString("O") : string.Empty))
            .ForMember(d => d.VerificationCode, opt => opt.MapFrom(s => s.VerificationCode ?? string.Empty))
            .ForMember(d => d.CountryName, opt => opt.MapFrom(s => s.CountryNavigation.Name));

        CreateMap<MatrixPositionDto, MatrixPositionMessage>(MemberList.None);

        CreateMap<AffiliateBtcDto, AffiliateBtcMessage>(MemberList.None)
            .ForMember(d => d.Id, opt => opt.MapFrom(s => (int)s.Id))
            .ForMember(d => d.AffiliateId, opt => opt.MapFrom(s => (int)s.AffiliateId))
            .ForMember(d => d.NetworkId, opt => opt.MapFrom(s => (int)(s.NetworkId ?? 0)));

        CreateMap<AffiliatePersonalNetwork, PersonalNetworkMessage>(MemberList.None)
            .ForMember(d => d.Id, opt => opt.MapFrom(s => (int)s.Id))
            .ForMember(d => d.Latitude,
                opt => opt.MapFrom(s => s.Latitude.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.Longitude,
                opt => opt.MapFrom(s => s.Longitude.ToString(CultureInfo.InvariantCulture)));
    }
}

