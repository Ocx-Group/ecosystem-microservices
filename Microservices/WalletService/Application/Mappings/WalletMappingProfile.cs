using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDetailDto;
using Ecosystem.WalletService.Domain.DTOs.ResultEcoPoolLevelsDto;
using Ecosystem.WalletService.Domain.DTOs.ResultsEcoPoolDto;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Application.Mappings;

public class WalletMappingProfile : Profile
{
    public WalletMappingProfile()
    {
        // Entity -> DTO
        CreateMap<WalletsWithdrawal, WalletWithDrawalDto>();
        CreateMap<WalletsWait, WalletWaitDto>();
        CreateMap<WalletsRetentionsConfig, WalletRetentionConfigDto>();
        CreateMap<WalletsPeriod, WalletPeriodDto>();
        CreateMap<WalletsHistory, WalletHistoryDto>();
        CreateMap<InvoicesDetail, InvoiceDetailDto>()
            .ForMember(dest => dest.Invoice, opt => opt.Ignore());
        CreateMap<ResultsModel2, ResultsEcoPoolDto>()
            .ForMember(dest => dest.ResultEcoPoolLevels, opt => opt.MapFrom(src => src.ResultsModel2Levels));
        CreateMap<ResultsModel2Level, ResultEcoPoolLevelsDto>();

        // Command -> Entity
        CreateMap<CreateWalletWithdrawalCommand, WalletsWithdrawal>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());

        CreateMap<CreateWalletWaitCommand, WalletsWait>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());

        CreateMap<CreateWalletHistoryCommand, WalletsHistory>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());
    }
}
