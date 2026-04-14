using System.Globalization;
using AutoMapper;
using Ecosystem.Grpc.Configuration;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Mappings;

public class ConfigurationGrpcMappingProfile : Profile
{
    public ConfigurationGrpcMappingProfile()
    {
        CreateMap<MatrixConfigMessage, MatrixConfiguration>(MemberList.None)
            .ForMember(d => d.Threshold,   opt => opt.MapFrom(src => ParseDecimal(src.Threshold)))
            .ForMember(d => d.FeeAmount,   opt => opt.MapFrom(src => ParseDecimal(src.FeeAmount)))
            .ForMember(d => d.MinWithdraw, opt => opt.MapFrom(src => ParseDecimal(src.MinWithdraw)))
            .ForMember(d => d.MaxWithdraw, opt => opt.MapFrom(src => ParseDecimal(src.MaxWithdraw)))
            .ForMember(d => d.RangeMin,    opt => opt.MapFrom(src => ParseDecimal(src.RangeMin)))
            .ForMember(d => d.RangeMax,    opt => opt.MapFrom(src => ParseDecimal(src.RangeMax)));
    }

    private static decimal ParseDecimal(string value)
        => decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0m;
}

