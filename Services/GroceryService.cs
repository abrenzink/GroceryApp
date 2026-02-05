using Microsoft.EntityFrameworkCore;
using Data;
using Models;

namespace Services;

public class GroceryService
{
  private readonly AppDbContext _context;

  public GroceryService(AppDbContext context)
  {
    _context = context;
  }

  // Obtain all products avaliables
  public async Task<List<GroceryItem>> GetAvailableProductsAsync()
  {
   
        try
        {
            Console.WriteLine("=== Consultando productos disponibles ===");
            
            var allItems = await _context.GroceryItems.ToListAsync();
            Console.WriteLine($"Total de productos en DB: {allItems.Count}");
            
            var availableItems = allItems.Where(item => item.IsAvailable).ToList();
            Console.WriteLine($"Productos disponibles: {availableItems.Count}");
            
            foreach (var item in availableItems)
            {
                Console.WriteLine($"- {item.Name} (ID: {item.Id}, Disponible: {item.IsAvailable})");
            }
            
            return availableItems;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR en GetAvailableProductsAsync: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
  }


}