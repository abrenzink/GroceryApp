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

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddCascadingAuthenticationState();



// Connection to SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
    options.UseSnakeCaseNamingConvention();
});

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
        // Crear la base de datos si no existe
        context.Database.EnsureCreated();
        Console.WriteLine("Database created/verified");

        // Inicializar datos
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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/test-users", async (AppDbContext db) =>
{
    return await db.Users.ToListAsync();
});

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});



app.Run();