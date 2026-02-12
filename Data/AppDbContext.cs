using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using GroceryApp.Models;

namespace GroceryApp.Data;

/// <summary>
/// Database context for the application, handling entity interactions and configurations.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Represents the Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Represents the GroceryItems table.
    /// </summary>
    public DbSet<GroceryItem> GroceryItems { get; set; }
    
    /// <summary>
    /// Represents the ShoppingCarts table.
    /// </summary>
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    
    /// <summary>
    /// Represents the CartItems table.
    /// </summary>
    public DbSet<CartItem> CartItems { get; set; }

    /// <summary>
    /// Configures the database context options.
    /// </summary>
    /// <param name="optionsBuilder">The builder being used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSnakeCaseNamingConvention();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);



        // Optional: if you want singular table names (not needed here, your tables are plural)
        // modelBuilder.UseSingularTableNames(false);

        // Optional: configure relationships explicitly if needed
        modelBuilder.Entity<ShoppingCart>()
            .HasOne(sc => sc.User)
            .WithMany()
            .HasForeignKey(sc => sc.UserId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.GroceryItem)
            .WithMany()
            .HasForeignKey(ci => ci.GroceryItemId);


        modelBuilder.Entity<GroceryItem>().ToTable("grocery_items");
    }

}
