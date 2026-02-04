using GroceryApp.Components;


//Includes needed for Database
using Microsoft.EntityFrameworkCore;
//using EFCore.NamingConventions;
using Data;
using Services;

var builder = WebApplication.CreateBuilder(args);

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
