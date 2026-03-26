using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.LeaderBoard;
using Ecosystem.AccountService.Domain.Models.CustomModels;

namespace Ecosystem.AccountService.Application.Mappings;

public class LeaderBoardMappingProfile : Profile
{
    public LeaderBoardMappingProfile()
    {
        CreateMap<ModelsFamilyTree, UserBinaryTreeDto>()
            .ForMember(d => d.Father, opt => opt.MapFrom(s => s.Father ?? 0));

        CreateMap<ModelsFamilyTree, UserUniLevelTreeDto>()
            .ForMember(d => d.id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.userName, opt => opt.MapFrom(s => s.UserName))
            .ForMember(d => d.level, opt => opt.MapFrom(s => s.Level))
            .ForMember(d => d.father, opt => opt.MapFrom(s => s.Father ?? 0));
    }
}
