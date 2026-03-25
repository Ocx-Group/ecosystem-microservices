using Ecosystem.AccountService.Application.Queries.Auth;
using AutoMapper;
using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Auth;

public class GetCountriesHandler : IRequestHandler<GetCountriesQuery, ICollection<CountryDto>>
{
    private readonly IUserAffiliateInfoRepository _userAffiliateInfoRepository;
    private readonly IMapper _mapper;

    public GetCountriesHandler(IUserAffiliateInfoRepository userAffiliateInfoRepository, IMapper mapper)
    {
        _userAffiliateInfoRepository = userAffiliateInfoRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _userAffiliateInfoRepository.GetCountries();
        return _mapper.Map<ICollection<CountryDto>>(countries);
    }
}
