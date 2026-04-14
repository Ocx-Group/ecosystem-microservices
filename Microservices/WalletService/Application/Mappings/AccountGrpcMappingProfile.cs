using System.Globalization;
using AutoMapper;
using Ecosystem.Grpc.Account;
using Ecosystem.WalletService.Domain.DTOs.AffiliateBtc;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Mappings;

public class AccountGrpcMappingProfile : Profile
{
    public AccountGrpcMappingProfile()
    {
        CreateMap<UserInfoMessage, UserInfoResponse>(MemberList.None)
            .ForMember(d => d.Status,
                opt => opt.MapFrom(s => (byte)s.Status))
            .ForMember(d => d.ActivationDate,
                opt => opt.MapFrom(s =>
                    string.IsNullOrEmpty(s.ActivationDate)
                        ? (DateTime?)null
                        : DateTime.Parse(s.ActivationDate, CultureInfo.InvariantCulture)))
            .ForMember(d => d.Country,
                opt => opt.MapFrom(s => new CountryNavigation { Name = s.CountryName }));

        CreateMap<MatrixPositionMessage, MatrixPositionDto>(MemberList.None);

        CreateMap<AffiliateBtcMessage, AffiliateBtcDto>(MemberList.None);

        CreateMap<PersonalNetworkMessage, PersonalNetwork>(MemberList.None)
            .ForMember(d => d.status,
                opt => opt.MapFrom(s => (byte)s.Status))
            .ForMember(d => d.latitude,
                opt => opt.MapFrom(s => ParseDecimal(s.Latitude)))
            .ForMember(d => d.longitude,
                opt => opt.MapFrom(s => ParseDecimal(s.Longitude)));
    }

    private static decimal ParseDecimal(string value)
        => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
}

