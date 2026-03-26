namespace Ecosystem.AccountService.Application.Helpers;

public static class VerificationCodeHelper
{
    public static string GenerateVerificationCode(long userId = 0)
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();
        return userId > 0 ? $"{userId}-{code}" : code;
    }
}
