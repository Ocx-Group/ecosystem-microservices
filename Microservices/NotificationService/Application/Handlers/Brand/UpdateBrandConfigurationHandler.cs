using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Application.Commands.Brand;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Brand;

public class UpdateBrandConfigurationHandler
    : IRequestHandler<UpdateBrandConfigurationCommand, BrandConfigurationDto>
{
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;

    public UpdateBrandConfigurationHandler(
        IBrandConfigurationRepository brandRepository,
        ITenantContext tenantContext,
        IMapper mapper)
    {
        _brandRepository = brandRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
    }

    public async Task<BrandConfigurationDto> Handle(
        UpdateBrandConfigurationCommand request, CancellationToken cancellationToken)
    {
        var brand = await _brandRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Brand configuration with id {request.Id} not found");

        if (brand.BrandId != _tenantContext.TenantId)
            throw new UnauthorizedAccessException(
                $"Brand configuration {request.Id} does not belong to the current tenant");

        if (request.Name is not null) brand.Name = request.Name;
        if (request.SenderName is not null) brand.SenderName = request.SenderName;
        if (request.SenderEmail is not null) brand.SenderEmail = request.SenderEmail;
        if (request.SupportEmail is not null) brand.SupportEmail = request.SupportEmail;
        if (request.ClientUrl is not null) brand.ClientUrl = request.ClientUrl;
        if (request.IsActive.HasValue) brand.IsActive = request.IsActive.Value;

        var updated = await _brandRepository.UpdateAsync(brand);
        return _mapper.Map<BrandConfigurationDto>(updated);
    }
}
