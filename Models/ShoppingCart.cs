namespace GroceryApp.Models;

public class ShoppingCart
{
  public int Id { get; set; }
  public int UserId { get; set; }
  public string Status { get; set; } = "Active";  // 'Active', 'Completed', 'Abandoned'
  public decimal TotalAmount { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  // Navigation properties (optional but useful)
  public User? User { get; set; }
  public List<CartItem> Items { get; set; } = new();
  public decimal GetTotalPrice() => Items.Sum(i => i.GetSubtotal());
  public string GetFormattedTotalPrice() => GetTotalPrice().ToString("0.00");
}