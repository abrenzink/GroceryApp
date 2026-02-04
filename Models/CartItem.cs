namespace Models;

public class CartItem
{
  public int Id { get; set; }
  public int ShoppingCartId { get; set; }
  public int GroceryItemId { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal Subtotal { get; set; }
  public DateTime AddedAt { get; set; }

  // Navigation properties (optional)
  public ShoppingCart? ShoppingCart { get; set; }
  public GroceryItem? GroceryItem { get; set; }
}