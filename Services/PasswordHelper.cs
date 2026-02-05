using System.Security.Cryptography;
using System.Text;

namespace GroceryApp.Services;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var md5 = MD5.Create();
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        // Converte para hexadecimal minúsculo para compatibilidade com md5() padrão
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        // Comparação insensível a maiúsculas/minúsculas para maior robustez
        return string.Equals(hash, storedHash, StringComparison.OrdinalIgnoreCase);
    }
}
