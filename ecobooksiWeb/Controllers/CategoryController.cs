using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecobooksi.Web.Controllers
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
            var categoris = await _categoryRepository.GetAllAsync();

            if (ModelState.IsValid)
            {
                return View("Index", categoris);
            }

            return NotFound();
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> Details(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(category => category.CategoryId == categoryId);

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
        public async Task<IActionResult> CreateAndSave(Category category)
        {
            if (category.CategoryName == category.DisplayOrder.ToString())
                ModelState.AddModelError("CategoryName", "Display Order cannot exactly match the Category Name!");

            if (ModelState.IsValid)
            {
                _categoryRepository.Create(category);

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
            var currentCategory = await _categoryRepository.GetAsync(category => category.CategoryId == categoryId);
            if (currentCategory is null)
                return NotFound();

            return View("Edit", currentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAndSave(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                _categoryRepository.Save();

                // for notification purposes
                TempData["success"] = "Category Updated Successfully!";

                return RedirectToAction("Index");
            }
            return View("Edit", category);
        }


        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();

            // send the currrent category with the opend view 
            var currentCategory = await _categoryRepository.GetAsync(category => category.CategoryId == categoryId);
            if (currentCategory is null)
                return NotFound();


            return View("Delete", currentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndSave(int categoryId)
        {
            var currentCategory = await _categoryRepository.GetAsync(category => category.CategoryId == categoryId);

            if (currentCategory is null)
                return NotFound();

            await _categoryRepository.Delete(currentCategory);

            // for notification purposes
            TempData["success"] = "Category Deleted Successfully!";

            return RedirectToAction("Index");
        }
    }
}
