using AutoMapper;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Models;

namespace Ecosystem.NotificationService.Application.Mappings;

public class NotificationMappingProfile : Profile
{
    public NotificationMappingProfile()
    {
        CreateMap<EmailTemplate, EmailTemplateDto>();
        CreateMap<BrandConfiguration, BrandConfigurationDto>();
    }
}
