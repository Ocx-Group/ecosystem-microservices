using AutoMapper;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Application.Commands.MatrixEarnings;
using Ecosystem.WalletService.Application.Commands.PaymentTransaction;
using Ecosystem.WalletService.Application.Commands.Pagadito;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Application.Commands.WalletWait;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDetailDto;
using Ecosystem.WalletService.Domain.DTOs.MatrixEarningDto;
using Ecosystem.WalletService.Domain.DTOs.MatrixQualificationDto;
using Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;
using Ecosystem.WalletService.Domain.DTOs.ResultEcoPoolLevelsDto;
using Ecosystem.WalletService.Domain.DTOs.ResultsEcoPoolDto;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.PagaditoRequest;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

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
        CreateMap<Domain.Models.Wallet, WalletDto>()
            .ForMember(d => d.Id, map => map.MapFrom(src => (int)src.Id))
            .ForMember(d => d.AffiliateId, map => map.MapFrom(src => (int)src.AffiliateId))
            .ForMember(d => d.UserId, map => map.MapFrom(src => src.UserId ?? 0))
            .ForMember(d => d.Credit, map => map.MapFrom(src => (double)(src.Credit ?? 0m)))
            .ForMember(d => d.Debit, map => map.MapFrom(src => (double)(src.Debit ?? 0m)))
            .ForMember(d => d.Deferred, map => map.MapFrom(src => src.Deferred.HasValue ? (double?)src.Deferred.Value : null))
            .ForMember(d => d.Status, map => map.MapFrom(src => src.Status ?? false));
        CreateMap<MatrixEarning, MatrixEarningDto>();
        CreateMap<MatrixQualification, MatrixQualificationDto>();
        CreateMap<Transaction, PaymentTransactionDto>();
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

        CreateMap<CreateMatrixEarningCommand, MatrixEarning>()
            .ForMember(d => d.EarningId, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());

        CreateMap<CreatePaymentTransactionCommand, Transaction>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());

        CreateMap<CreatePagaditoTransactionCommand, CreatePagaditoTransactionRequest>();
        CreateMap<PagaditoTransactionDetailRequest, PagaditoTransactionDetail>()
            .ForMember(d => d.Description, map => map.MapFrom(src => src.Description ?? string.Empty))
            .ForMember(d => d.Price, map => map.MapFrom(src => src.Price ?? 0m))
            .ForMember(d => d.UrlProduct, map => map.MapFrom(src => src.UrlProduct ?? string.Empty));
        CreateMap<CreatePagaditoTransactionRequest, CreatePagaditoTransaction>()
            .ForMember(d => d.Token, map => map.Ignore())
            .ForMember(d => d.Ern, map => map.Ignore());

        CreateMap<CreateCoinPayTransactionCommand, CreateTransactionRequest>();
        CreateMap<CreateCoinPayChannelCommand, CreateChannelRequest>();
        CreateMap<CreateCoinPayAddressCommand, CreateAddresRequest>();
        CreateMap<SendCoinPayFundsCommand, SendFundRequest>();
        CreateMap<CreateCoinPaymentCommand, ConPaymentRequest>();
    }
}
