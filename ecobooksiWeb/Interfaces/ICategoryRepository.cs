using ecobooksiWeb.Dtos;
using ecobooksiWeb.Models;

namespace ecobooksiWeb.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<ICollection<Category>> GetCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int categoryId);
        public Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        public Task<Category?> UpdateCategoryAsync(int categoryId, UpdateCategoryDto updateCategoryDto);
        public Task DeleteCategoryAsync(int categoryId);
        public Task Save();
    }
}
