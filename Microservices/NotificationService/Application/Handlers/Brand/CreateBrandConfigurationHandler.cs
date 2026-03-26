using AutoMapper;
using Ecosystem.NotificationService.Application.Commands.Brand;
using Ecosystem.NotificationService.Application.DTOs;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using MediatR;

namespace Ecosystem.NotificationService.Application.Handlers.Brand;

public class CreateBrandConfigurationHandler
    : IRequestHandler<CreateBrandConfigurationCommand, BrandConfigurationDto>
{
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IMapper _mapper;

    public CreateBrandConfigurationHandler(IBrandConfigurationRepository brandRepository, IMapper mapper)
    {
        _brandRepository = brandRepository;
        _mapper = mapper;
    }

    public async Task<BrandConfigurationDto> Handle(
        CreateBrandConfigurationCommand request, CancellationToken cancellationToken)
    {
        var existing = await _brandRepository.GetByBrandIdAsync(request.BrandId);
        if (existing is not null)
            throw new InvalidOperationException($"Brand configuration for brandId {request.BrandId} already exists");

        var brand = new BrandConfiguration
        {
            BrandId = request.BrandId,
            Name = request.Name,
            SenderName = request.SenderName,
            SenderEmail = request.SenderEmail,
            SupportEmail = request.SupportEmail,
            ClientUrl = request.ClientUrl
        };

        var created = await _brandRepository.CreateAsync(brand);
        return _mapper.Map<BrandConfigurationDto>(created);
    }
}
