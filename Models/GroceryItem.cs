namespace GroceryApp.Models;

public class GroceryItem
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public string? Description { get; set; }
  public string? Category { get; set; }
  public string? ImageUrl { get; set; }
  public int Stock { get; set; }
  public bool IsAvailable { get; set; }
  public int AdminId { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}