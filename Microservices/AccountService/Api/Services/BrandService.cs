using Ecosystem.AccountService.Application.Interfaces;

namespace Ecosystem.AccountService.Api.Services;

public class BrandService : IBrandService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BrandService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public long BrandId
    {
        get
        {
            var brandId = _httpContextAccessor.HttpContext?.Items["brandId"];
            return brandId is null ? 0 : Convert.ToInt64(brandId);
        }
    }
}
