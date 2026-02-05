using GroceryApp.Components;


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



// Connection to CSE325 database PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("AivenPostgres");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("AivenPostgres"));

    options.UseSnakeCaseNamingConvention();
});


builder.Services.AddScoped<GroceryService>();

var app = builder.Build();

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
