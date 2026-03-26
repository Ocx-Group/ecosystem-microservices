using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Matrix;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Application.Mappings;

public class MatrixMappingProfile : Profile
{
    public MatrixMappingProfile()
    {
        CreateMap<UniLevelFamilyTree, UserUniLevelTreeDto>()
            .ForMember(d => d.id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.userName, opt => opt.MapFrom(s => s.UserName))
            .ForMember(d => d.level, opt => opt.MapFrom(s => s.Level))
            .ForMember(d => d.father, opt => opt.MapFrom(s => s.Father))
            .ForMember(d => d.grading, opt => opt.MapFrom(s => s.Grading))
            .ForMember(d => d.imageProfileUrl, opt => opt.MapFrom(s => s.ImageProfileUrl));

        CreateMap<MatrixTree, MatrixDto>()
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Username, opt => opt.MapFrom(s => s.Username))
            .ForMember(d => d.Level, opt => opt.MapFrom(s => s.Level))
            .ForMember(d => d.Father, opt => opt.MapFrom(s => s.Father))
            .ForMember(d => d.ImageProfileUrl, opt => opt.MapFrom(s => s.ImageProfileUrl))
            .ForMember(d => d.QualificationCount, opt => opt.MapFrom(s => s.QualificationCount));
    }
}
