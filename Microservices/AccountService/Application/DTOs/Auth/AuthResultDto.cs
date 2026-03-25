namespace Ecosystem.AccountService.Application.DTOs.Auth;

public class AuthResultDto
{
    public UserDto? User { get; set; }
    public UsersAffiliatesDto? Affiliate { get; set; }

    public bool IsAuthenticated => User is not null || Affiliate is not null;
}