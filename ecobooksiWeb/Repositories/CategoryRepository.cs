using ecobooksiWeb.Data;
using ecobooksiWeb.Dtos;
using ecobooksiWeb.Interfaces;
using ecobooksiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ecobooksiWeb.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            if (categories is null)
                return [];

            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }
        public async Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            // use auto mapper later 
            var category = new Category
            {
                CategoryName = createCategoryDto.CategoryName,
                DisplayOrder = createCategoryDto.DisplayOrder
            };

            await _context.Categories.AddAsync(category);
            await Save();

            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int categoryId, UpdateCategoryDto updateCategoryDto)
        {
            var currentCategory = await _context.Categories.FindAsync(categoryId);

            if (currentCategory is null)
                return null;

            currentCategory.CategoryName = updateCategoryDto.CategoryName;
            currentCategory.DisplayOrder = updateCategoryDto.DisplayOrder;

            await Save();

            return currentCategory;
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var currentCategory = _context.Categories.Find(categoryId);

            if (currentCategory is null)
                throw new ArgumentNullException(nameof(currentCategory), "Category not found.");

            _context.Categories.Remove(currentCategory);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

    }
}
