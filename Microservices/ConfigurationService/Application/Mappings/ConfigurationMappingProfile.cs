using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Commands.Concept;
using Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;
using Ecosystem.ConfigurationService.Application.Commands.Grading;
using Ecosystem.ConfigurationService.Application.Commands.Incentive;
using Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Application.Mappings;

public class ConfigurationMappingProfile : Profile
{
    public ConfigurationMappingProfile()
    {
        // Entity -> DTO
        CreateMap<ConceptConfigurations, ConceptConfigurationDto>();
        CreateMap<Concepts, ConceptDto>();
        CreateMap<Configurations, ConfigurationDto>();
        CreateMap<PaymentGroups, PaymentGroupsDto>();
        CreateMap<Gradings, GradingDto>();
        CreateMap<Incentives, IncentiveDto>();
        CreateMap<MatrixConfiguration, MatrixConfigDto>();

        // Command -> Entity
        CreateMap<CreateConceptCommand, Concepts>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.Status, map => map.Ignore())
            .ForMember(d => d.DateConcept, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore());

        CreateMap<CreateConceptConfigurationCommand, ConceptConfigurations>();
        CreateMap<CreateGradingCommand, Gradings>();
        CreateMap<CreatePaymentGroupCommand, PaymentGroups>()
            .ForMember(d => d.Id, map => map.Ignore())
            .ForMember(d => d.CreatedAt, map => map.Ignore())
            .ForMember(d => d.UpdatedAt, map => map.Ignore())
            .ForMember(d => d.DeletedAt, map => map.Ignore())
            .ForMember(d => d.Status, map => map.Ignore());

        CreateMap<CreateIncentiveCommand, Incentives>();
    }
}
