using GroceryApp.Models;

namespace GroceryApp.Services;

public class CartState
{
    public ShoppingCart Cart { get; private set; } = new ShoppingCart();

    public void incrementQuantity (GroceryItem item)
    {
        CartItem cartItem = Cart.Items.First(i => i.Id == item.Id);

        if ( cartItem != null ) cartItem.Quantity += 1;
    }

    public void addToCart(GroceryItem item)
    {
        Cart.Items.Add(new CartItem
        {
            Quantity = 1,
            GroceryItem = item
        });
    }

    public void deleteFromCart(CartItem item)
    {
        Cart.Items.Remove(item);
    }

    public void resetCart()
    {
        Cart = new ShoppingCart();
    }
}
