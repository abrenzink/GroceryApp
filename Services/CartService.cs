using GroceryApp.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GroceryApp.Services;

/// <summary>
/// Manages the state of the shopping cart, including items, visibility, and user association.
/// </summary>
public class CartState
{
    private readonly AuthenticationStateProvider _authStateProvider;
    
    /// <summary>
    /// Gets the current shopping cart.
    /// </summary>
    public ShoppingCart Cart { get; private set; } = new();
    
    /// <summary>
    /// Event triggered when the cart state changes.
    /// </summary>
    public event Action? OnChange;
    
    /// <summary>
    /// Gets a value indicating whether the cart is currently visible/open.
    /// </summary>
    public bool showCart { get; private set; } = false;

    /// <summary>
    /// Opens the cart view.
    /// </summary>
    public void openCart() 
    {
        showCart = true;
        NotifyStateChanged();
    }

    /// <summary>
    /// Closes the cart view.
    /// </summary>
    public void closeCart() 
    {
        showCart = false;
        NotifyStateChanged();
    }

    public CartState(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    /// <summary>
    /// Initializes the cart state, associating it with the logged-in user if applicable.
    /// </summary>
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
    
    /// <summary>
    /// Checks if a specific grocery item is already in the cart.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item is in the cart; otherwise, false.</returns>
    public bool isInCart (GroceryItem item)
    {
        return Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id) != null;
    }

    /// <summary>
    /// Increments the quantity of a specific grocery item in the cart.
    /// </summary>
    /// <param name="item">The item to increment.</param>
    public void incrementQuantity (GroceryItem item)
    {
        CartItem cartItem = Cart.Items.FirstOrDefault(i => i.GroceryItem.Id == item.Id);

        if ( cartItem != null ) 
        {
            cartItem.Quantity++;
            showCart = true;
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// Decrements the quantity of a specific grocery item in the cart. Removes the item if quantity drops below 1.
    /// </summary>
    /// <param name="item">The item to decrement.</param>
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

    /// <summary>
    /// Adds a grocery item to the cart or increments its quantity if it already exists.
    /// </summary>
    /// <param name="item">The grocery item to add.</param>
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
        showCart = true;
        NotifyStateChanged();
    }

    /// <summary>
    /// Removes a specific cart item from the cart.
    /// </summary>
    /// <param name="item">The cart item to remove.</param>
    public void deleteFromCart(CartItem item)
    {
        Cart.Items.Remove(item);
        NotifyStateChanged();
    }

    /// <summary>
    /// Resets the cart to an empty state, preserving the user association.
    /// </summary>
    public void resetCart()
    {
        var userId = Cart.UserId;
        Cart = new ShoppingCart { UserId = userId };
        NotifyStateChanged();
    }

    /// <summary>
    /// Invokes the OnChange event to notify subscribers of state changes.
    /// </summary>
    private void NotifyStateChanged() => OnChange?.Invoke();
}
