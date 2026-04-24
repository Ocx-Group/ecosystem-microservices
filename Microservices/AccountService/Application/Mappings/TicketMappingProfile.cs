using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Ecosystem.Domain.Core.Storage;

namespace Ecosystem.AccountService.Application.Mappings;

public class TicketMappingProfile : Profile
{
    public TicketMappingProfile()
    {
        CreateMap<TicketCategory, TicketCategoriesDto>();

        CreateMap<Domain.Models.Ticket, TicketDto>();
        CreateMap<TicketImage, TicketImagesDto>()
            .ForMember(d => d.ImagePath, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.ImagePath) ?? string.Empty));
        CreateMap<MessageDetails, TicketMessageDto>()
            .ForMember(d => d.ImageProfileUrl, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.ImageProfileUrl) ?? string.Empty));
        CreateMap<TicketMessage, TicketMessageDto>();

        CreateMap<CreateTicketCommand, Domain.Models.Ticket>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.TicketImages, opt => opt.MapFrom(s => s.Images))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

        CreateMap<TicketImageItem, TicketImage>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.TicketId, opt => opt.MapFrom(s => s.TicketId))
            .ForMember(d => d.ImagePath, opt => opt.MapFrom(s => s.ImagePath))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));

        CreateMap<SendTicketMessageCommand, TicketMessage>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.TicketId, opt => opt.MapFrom(s => s.TicketId))
            .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.UserId))
            .ForMember(d => d.MessageContent, opt => opt.MapFrom(s => s.MessageContent))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(d => d.UpdatedAt, opt => opt.MapFrom(_ => DateTime.Now));
    }
}
