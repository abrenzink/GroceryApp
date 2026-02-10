using GroceryApp.Models;

namespace GroceryApp.Services
{
  public interface IGroceryItemService
  {
    Task<List<GroceryItem>> GetAllAsync();
    Task<GroceryItem?> GetByIdAsync(int id);
    Task<GroceryItem> CreateAsync(GroceryItem item);
    Task<GroceryItem?> UpdateAsync(GroceryItem item);
    Task<bool> DeleteAsync(int id);
    Task<List<GroceryItem>> GetByCategoryAsync(string category);
    Task<List<GroceryItem>> SearchAsync(string searchTerm);
  }
}