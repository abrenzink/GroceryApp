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
    return await _context.GroceryItems
        .Where(item => item.IsAvailable)
        .ToListAsync();
  }


}