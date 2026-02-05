using GroceryApp.Components;
using GroceryApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;


//Includes needed for Database
using Microsoft.EntityFrameworkCore;
//using EFCore.NamingConventions;
using Data;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("PostgresLocal")
    )
);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();



// Connection to SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteConnection"));
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddScoped<GroceryService>();

var app = builder.Build();

// ========================================
// CREATE A DATABASE IF NOT EXIST
// ========================================
Console.WriteLine("=== INICIANDO CONFIGURACIÓN DE BASE DE DATOS ===");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        Console.WriteLine("Creando base de datos...");
        // Crear la base de datos si no existe
        context.Database.EnsureCreated();
        Console.WriteLine("Base de datos creada/verificada");

        // Inicializar datos
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR al crear la base de datos: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    }
}

Console.WriteLine("=== CONFIGURACIÓN DE BASE DE DATOS COMPLETADA ===");
// ========================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
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
