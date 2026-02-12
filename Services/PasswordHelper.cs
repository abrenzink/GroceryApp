using System.Security.Cryptography;
using System.Text;

namespace GroceryApp.Services;

/// <summary>
/// Provides helper methods for password hashing and verification using SHA256.
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Hashes a password string using SHA256.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The hashed password as a lowercase hex string.</returns>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        // Convert to lowercase hex for compatibility
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    /// <summary>
    /// Verifies if a password matches a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="storedHash">The stored hash to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public static bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        // Case-insensitive comparison for robustness
        return string.Equals(hash, storedHash, StringComparison.OrdinalIgnoreCase);
    }
}
