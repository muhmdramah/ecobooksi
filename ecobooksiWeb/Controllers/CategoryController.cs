using ecobooksiWeb.Dtos;
using ecobooksiWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ecobooksiWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoris = await _categoryRepository.GetCategoriesAsync();

            if (ModelState.IsValid)
            {
                return View("Index", categoris);
            }

            return NotFound();
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> Details(int categoryId)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (ModelState.IsValid)
            {
                return View("Details", category);
            }

            return NotFound();
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndSave(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto.CategoryName == createCategoryDto.DisplayOrder.ToString())
                ModelState.AddModelError("CategoryName", "Display Order cannot exactly match the Category Name!");

            //if (createCategoryDto.CategoryName != null && createCategoryDto.CategoryName.ToLower() != "test")
            //    ModelState.AddModelError("", "Category Name is invaild!");

            if (ModelState.IsValid)
            {
                await _categoryRepository.CreateCategoryAsync(createCategoryDto);

                // for notification purposes
                TempData["success"] = "Category Created Successfully!";

                return RedirectToAction("Index", "Category");
            }

            return View("Create");
        }


        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();

            // send the currrent category with the opend view 
            var currentCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (currentCategory is null)
                return NotFound();

            // use auto mapper later 
            var updateDto = new UpdateCategoryDto
            {
                CategoryId = currentCategory.CategoryId,
                CategoryName = currentCategory.CategoryName,
                DisplayOrder = currentCategory.DisplayOrder
            };

            return View("Edit", updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAndSave(int categoryId, UpdateCategoryDto updateCategoryDto)
        {
            if (ModelState.IsValid)
            {
                await _categoryRepository.UpdateCategoryAsync(categoryId, updateCategoryDto);
                await _categoryRepository.Save();

                // for notification purposes
                TempData["success"] = "Category Updated Successfully!";

                return RedirectToAction("Index");
            }
            return View("Edit", updateCategoryDto);
        }


        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();

            // send the currrent category with the opend view 
            var currentCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId);
            if (currentCategory is null)
                return NotFound();

            // use auto mapper later 
            var deleteDto = new DeleteCategoryDto
            {
                CategoryId = currentCategory.CategoryId,
                CategoryName = currentCategory.CategoryName,
                DisplayOrder = currentCategory.DisplayOrder
            };

            return View("Delete", deleteDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAndSave(int categoryId)
        {
            var currentCategory = _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (currentCategory is null)
                return NotFound();

            _categoryRepository.DeleteCategoryAsync(categoryId);

            // for notification purposes
            TempData["success"] = "Category Deleted Successfully!";

            return RedirectToAction("Index");
        }
    }
}
