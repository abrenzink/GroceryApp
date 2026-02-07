using GroceryApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GroceryApp.Services;

public class CartState
{
    private readonly AuthenticationStateProvider _authStateProvider;
    public ShoppingCart Cart { get; private set; } = new();
    public event Action? OnChange;
    public bool showCart { get; private set; } = false;

    public void openCart() => showCart = true;
    public void closeCart() => showCart = false;
    public CartState(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task InitializeAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity?.IsAuthenticated == true)
        {
            // Assuming the NameIdentifier claim holds the user ID if it's an int, 
            // otherwise we might need to look it up by email.
            // For this simple app, let's try to parse NameIdentifier or another claim if available.
            // Adjust based on how login is implemented.
             var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
             if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
             {
                 Cart.UserId = userId;
             }
        }
    }
    
    
    public bool isInCart (GroceryItem item)
    {
        return Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id) != null;
    }

    public void incrementQuantity (GroceryItem item)
    {
        CartItem cartItem = Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id);

        if ( cartItem != null ) 
        {
            cartItem.Quantity++;
            NotifyStateChanged();
        }
    }

    public void decrementQuantity (GroceryItem item)
    {
        CartItem cartItem = Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id);

        if (cartItem != null && cartItem.Quantity < 2)
        {
            deleteFromCart(cartItem);
        }

        if ( cartItem != null ) 
        {
            cartItem.Quantity--;
            NotifyStateChanged();
        }
    }

    public void addToCart(GroceryItem item)
    {
        Console.WriteLine("Add To Cart");
        
        var existingItem = Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += 1;
        }
        else
        {
            Cart.Items.Add(new CartItem
            {
                Quantity = 1,
                GroceryItem = item,
                ShoppingCart = Cart, // Maintain reference
                ShoppingCartId = Cart.Id
            });
        }
        
        Console.WriteLine(Cart.ToString());
        NotifyStateChanged();
    }

    public void deleteFromCart(CartItem item)
    {
        Cart.Items.Remove(item);
        NotifyStateChanged();
    }

    public void resetCart()
    {
        var userId = Cart.UserId;
        Cart = new ShoppingCart { UserId = userId };
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
