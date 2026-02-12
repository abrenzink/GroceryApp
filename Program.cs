using GroceryApp.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using GroceryApp.Data;
using GroceryApp.Services;
using EFCore.NamingConventions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure authentication with Cookies
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });

// Add cascading authentication state for Blazor components
builder.Services.AddCascadingAuthenticationState();



// Connection to SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
    // Use snake case naming convention for database columns
    options.UseSnakeCaseNamingConvention();
});

// Register application services
builder.Services.AddScoped<IGroceryItemService, GroceryItemService>();
builder.Services.AddScoped<GroceryService>();
builder.Services.AddScoped<CartState>();

var app = builder.Build();

// ========================================
// CREATE A DATABASE IF NOT EXIST
// ========================================
Console.WriteLine("=== STARTING DATABASE CONFIGURATION ===");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        Console.WriteLine("Creating database...");
        // Create the database if it doesn't exist
        context.Database.EnsureCreated();
        Console.WriteLine("Database created/verified");

        // Initialize data
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR creating database: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    }
}

Console.WriteLine("=== DATABASE CONFIGURATION COMPLETED ===");
// ========================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirect middleware for 404 errors
app.UseStatusCodePagesWithRedirects("/not-found");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Test endpoint to retrieve users
app.MapGet("/test-users", async (AppDbContext db) =>
{
    return await db.Users.ToListAsync();
});

// Logout endpoint
app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});



app.Run();