namespace Ecosystem.AccountService.Application.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
        => BCrypt.Net.BCrypt.HashPassword(password);

    public static bool VerifyPassword(string? hash, string password)
    {
        if (string.IsNullOrEmpty(hash)) return false;
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}