using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;
using Models;

namespace Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets for the main entities
    public DbSet<User> Users { get; set; }
    public DbSet<GroceryItem> GroceryItems { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

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