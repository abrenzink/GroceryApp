namespace GroceryApp.Data;

using Microsoft.EntityFrameworkCore;
using GroceryApp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    // public DbSet<GroceryItem> GroceryItems => Set<GroceryItem>();
    // public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    // public DbSet<CartItem> CartItems => Set<CartItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users");
        // modelBuilder.Entity<GroceryItem>().ToTable("grocery_items");
        // modelBuilder.Entity<ShoppingCart>().ToTable("shopping_carts");
        // modelBuilder.Entity<CartItem>().ToTable("cart_items");
    }
}
