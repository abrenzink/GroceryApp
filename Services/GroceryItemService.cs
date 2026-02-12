using Microsoft.EntityFrameworkCore;
using GroceryApp.Data;
using GroceryApp.Models;

namespace GroceryApp.Services
{
  /// <summary>
  /// Service for managing grocery items, including CRUD operations and search functionality.
  /// </summary>
  public class GroceryItemService : IGroceryItemService
  {
    private readonly AppDbContext _context;

    public GroceryItemService(AppDbContext context)
    {
      _context = context;
    }

    /// <summary>
    /// Retrieves all grocery items from the database, ordered by creation date.
    /// </summary>
    /// <returns>A list of all grocery items.</returns>
    public async Task<List<GroceryItem>> GetAllAsync()
    {
      return await _context.GroceryItems
          .Include(g => g.Admin)
          .OrderByDescending(g => g.CreatedAt)
          .ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific grocery item by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the item.</param>
    /// <returns>The grocery item if found; otherwise, null.</returns>
    public async Task<GroceryItem?> GetByIdAsync(int id)
    {
      return await _context.GroceryItems
          .Include(g => g.Admin)
          .FirstOrDefaultAsync(g => g.Id == id);
    }

    /// <summary>
    /// Creates a new grocery item in the database.
    /// </summary>
    /// <param name="item">The grocery item to create.</param>
    /// <returns>The created grocery item.</returns>
    public async Task<GroceryItem> CreateAsync(GroceryItem item)
    {
      item.CreatedAt = DateTime.UtcNow;
      item.UpdatedAt = DateTime.UtcNow;

      _context.GroceryItems.Add(item);
      await _context.SaveChangesAsync();

      return item;
    }

    /// <summary>
    /// Updates an existing grocery item.
    /// </summary>
    /// <param name="item">The grocery item with updated values.</param>
    /// <returns>The updated grocery item if successful; otherwise, null.</returns>
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

    /// <summary>
    /// Deletes a grocery item by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the item to delete.</param>
    /// <returns>True if the item was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteAsync(int id)
    {
      var item = await _context.GroceryItems.FindAsync(id);

      if (item == null)
        return false;

      _context.GroceryItems.Remove(item);
      await _context.SaveChangesAsync();

      return true;
    }

    /// <summary>
    /// Retrieves available grocery items belonging to a specific category.
    /// </summary>
    /// <param name="category">The category name.</param>
    /// <returns>A list of grocery items in the specified category.</returns>
    public async Task<List<GroceryItem>> GetByCategoryAsync(string category)
    {
      return await _context.GroceryItems
          .Where(g => g.Category == category && g.IsAvailable)
          .ToListAsync();
    }

    /// <summary>
    /// Searches for grocery items by name or description.
    /// </summary>
    /// <param name="searchTerm">The term to search for.</param>
    /// <returns>A list of grocery items matching the search term.</returns>
    public async Task<List<GroceryItem>> SearchAsync(string searchTerm)
    {
      return await _context.GroceryItems
          .Where(g => g.Name.Contains(searchTerm) ||
                     (g.Description != null && g.Description.Contains(searchTerm)))
          .ToListAsync();
    }
  }
}