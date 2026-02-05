using Models;

namespace Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        Console.WriteLine("=== Initializing Database ===");

        // Ensure the DB exists
        context.Database.EnsureCreated();
        Console.WriteLine("Database created/verified");

        // If users exist, do nothing
        if (context.Users.Any())
        {
            Console.WriteLine($"The database already has {context.Users.Count()} users");
            Console.WriteLine($"The database already has {context.GroceryItems.Count()} products");
            return;
        }

        Console.WriteLine("Inserting initial data...");

        // Add users
        var users = new User[]
        {
            new User
            {
                Name = "Admin User",
                Email = "admin@grocery.com",
                PasswordHash = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                PasswordHash = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Jane Smith",
                Email = "jane@example.com",
                PasswordHash = "ed968e840d10d2d313a870bc131a4e2c311d7ad09bdf32b3418147221f51a6e2",
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
        Console.WriteLine($"Inserted {users.Length} users");

        // Add products
        var groceryItems = new GroceryItem[]
        {
            new GroceryItem
            {
                Name = "Red Apples",
                Price = 2.99m,
                Description = "Fresh imported red apples",
                ImageUrl = "https://images.pexels.com/photos/33660535/pexels-photo-33660535/free-photo-of-close-up-of-fresh-red-apples-on-dark-background.png?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1",
                Category = "Fruits",
                Stock = 100,
                IsAvailable = true,
                AdminId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GroceryItem
            {
                Name = "Whole Milk",
                Price = 3.49m,
                Description = "Whole milk 1 liter",
                ImageUrl = "https://m.media-amazon.com/images/I/61H3Ng4tztL.jpg",
                Category = "Dairy",
                Stock = 50,
                IsAvailable = true,
                AdminId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GroceryItem
            {
                Name = "Whole Wheat Bread",
                Price = 2.50m,
                Description = "Artisanal whole wheat bread",
                ImageUrl = "https://www.theperfectloaf.com/wp-content/uploads/2016/07/theperfectloaf-fifty-fifty-whole-wheat-sourdough-title-lighting.jpg",
                Category = "Bakery",
                Stock = 30,
                IsAvailable = true,
                AdminId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GroceryItem
            {
                Name = "Whole Chicken",
                Price = 8.99m,
                Description = "Fresh farm-raised whole chicken",
                ImageUrl = "https://primecutny.com/cdn/shop/files/Broiler_Chicken.jpg?v=1740504373",
                Category = "Meats",
                Stock = 20,
                IsAvailable = true,
                AdminId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new GroceryItem
            {
                Name = "White Rice",
                Price = 4.99m,
                Description = "White rice 1kg",
                ImageUrl = "https://m.media-amazon.com/images/I/41B0yDr5ivL.jpg",
                Category = "Grains",
                Stock = 80,
                IsAvailable = true,
                AdminId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.GroceryItems.AddRange(groceryItems);
        context.SaveChanges();
        Console.WriteLine($"Inserted {groceryItems.Length} products");
        Console.WriteLine("=== Database Initialized ===");
    }
}