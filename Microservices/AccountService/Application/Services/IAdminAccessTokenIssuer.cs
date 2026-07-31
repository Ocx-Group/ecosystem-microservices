using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Application.Services;

public interface IAdminAccessTokenIssuer
{
    string Issue(User user);
}
