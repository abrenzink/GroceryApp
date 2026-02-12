using Microsoft.EntityFrameworkCore;
using GroceryApp.Data;
using GroceryApp.Models;

namespace GroceryApp.Services;

/// <summary>
/// Service responsible for managing grocery product retrieval.
/// </summary>
public class GroceryService
{
  private readonly AppDbContext _context;

  public GroceryService(AppDbContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Retrieves all available grocery products from the database, logging the process.
  /// </summary>
  /// <returns>A list of available grocery items.</returns>
  public async Task<List<GroceryItem>> GetAvailableProductsAsync()
  {

    try
    {
      Console.WriteLine("=== Querying available products ===");

      var allItems = await _context.GroceryItems.ToListAsync();
      Console.WriteLine($"Total products in DB: {allItems.Count}");

      var availableItems = allItems.Where(item => item.IsAvailable).ToList();
      Console.WriteLine($"Available products: {availableItems.Count}");

      foreach (var item in availableItems)
      {
        Console.WriteLine($"- {item.Name} (ID: {item.Id}, Available: {item.IsAvailable})");
      }

      return availableItems;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"ERROR in GetAvailableProductsAsync: {ex.Message}");
      Console.WriteLine($"Stack Trace: {ex.StackTrace}");
      throw;
    }
  }


}