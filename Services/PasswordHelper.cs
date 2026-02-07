using System.Security.Cryptography;
using System.Text;

namespace GroceryApp.Services;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        // Convert to lowercase hex for compatibility
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        // Case-insensitive comparison for robustness
        return string.Equals(hash, storedHash, StringComparison.OrdinalIgnoreCase);
    }
}
