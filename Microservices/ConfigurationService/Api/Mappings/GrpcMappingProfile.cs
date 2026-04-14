using System.Globalization;
using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.Grpc.Configuration;

namespace Ecosystem.ConfigurationService.Api.Mappings;

public class GrpcMappingProfile : Profile
{
    public GrpcMappingProfile()
    {
        CreateMap<MatrixConfigDto, MatrixConfigMessage>()
            .ForMember(d => d.Threshold,   opt => opt.MapFrom(src => src.Threshold.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.FeeAmount,   opt => opt.MapFrom(src => src.FeeAmount.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.MinWithdraw, opt => opt.MapFrom(src => src.MinWithdraw.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.MaxWithdraw, opt => opt.MapFrom(src => src.MaxWithdraw.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.RangeMin,    opt => opt.MapFrom(src => src.RangeMin.ToString(CultureInfo.InvariantCulture)))
            .ForMember(d => d.RangeMax,    opt => opt.MapFrom(src => src.RangeMax.ToString(CultureInfo.InvariantCulture)));
    }
}

