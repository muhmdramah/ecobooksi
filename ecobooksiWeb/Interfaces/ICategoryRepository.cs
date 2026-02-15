
using ecobooksi.Models.Dtos;
using ecobooksi.Models.Models;

namespace ecobooksi.Web.Interfaces
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
