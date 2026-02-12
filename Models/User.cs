namespace GroceryApp.Models;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string PasswordHash { get; set; } = string.Empty;
  
  /// <summary>
  /// The user's role (e.g., 'Admin' or 'User').
  /// </summary>
  public string Role { get; set; } = string.Empty;
  public bool IsActive { get; set; }
  public DateTime CreatedAt { get; set; }
}
