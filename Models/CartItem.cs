namespace GroceryApp.Models;
using System.Linq;

public class CartItem
{
  public int Id { get; set; }
  public int ShoppingCartId { get; set; }
  public int GroceryItemId { get; set; }
  public int Quantity { get; set; }
  public decimal GetSubtotal() => Quantity * (GroceryItem?.Price ?? 0);
  public string GetSubtotalFormatted() => GetSubtotal().ToString("0.00");
  public DateTime AddedAt { get; set; }

  // Navigation properties (optional)
  public ShoppingCart? ShoppingCart { get; set; }
  public required GroceryItem GroceryItem { get; set; }
}