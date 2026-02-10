using Microsoft.EntityFrameworkCore;
using GroceryApp.Data;
using GroceryApp.Models;

namespace GroceryApp.Services
{
  public class GroceryItemService : IGroceryItemService
  {
    private readonly AppDbContext _context;

    public GroceryItemService(AppDbContext context)
    {
      _context = context;
    }

    public async Task<List<GroceryItem>> GetAllAsync()
    {
      return await _context.GroceryItems
          .Include(g => g.Admin)
          .OrderByDescending(g => g.CreatedAt)
          .ToListAsync();
    }

    public async Task<GroceryItem?> GetByIdAsync(int id)
    {
      return await _context.GroceryItems
          .Include(g => g.Admin)
          .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<GroceryItem> CreateAsync(GroceryItem item)
    {
      item.CreatedAt = DateTime.UtcNow;
      item.UpdatedAt = DateTime.UtcNow;

      _context.GroceryItems.Add(item);
      await _context.SaveChangesAsync();

      return item;
    }

    public async Task<GroceryItem?> UpdateAsync(GroceryItem item)
    {
      var existingItem = await _context.GroceryItems.FindAsync(item.Id);

      if (existingItem == null)
        return null;

      existingItem.Name = item.Name;
      existingItem.Price = item.Price;
      existingItem.Description = item.Description;
      existingItem.Category = item.Category;
      existingItem.ImageUrl = item.ImageUrl;
      existingItem.Stock = item.Stock;
      existingItem.IsAvailable = item.IsAvailable;
      existingItem.UpdatedAt = DateTime.UtcNow;

      await _context.SaveChangesAsync();

      return existingItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var item = await _context.GroceryItems.FindAsync(id);

      if (item == null)
        return false;

      _context.GroceryItems.Remove(item);
      await _context.SaveChangesAsync();

      return true;
    }

    public async Task<List<GroceryItem>> GetByCategoryAsync(string category)
    {
      return await _context.GroceryItems
          .Where(g => g.Category == category && g.IsAvailable)
          .ToListAsync();
    }

    public async Task<List<GroceryItem>> SearchAsync(string searchTerm)
    {
      return await _context.GroceryItems
          .Where(g => g.Name.Contains(searchTerm) ||
                     (g.Description != null && g.Description.Contains(searchTerm)))
          .ToListAsync();
    }
  }
}