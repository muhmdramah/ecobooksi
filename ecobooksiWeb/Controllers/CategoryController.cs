using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecobooksi.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categoris = await _unitOfWork.Categories.GetAllAsync();

            if (ModelState.IsValid)
            {
                return View("Index", categoris);
            }

            return NotFound();
        }

        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> Details(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.CategoryId == categoryId);

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
                await _unitOfWork.Categories.Create(category);
                _unitOfWork.Complete();

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
            var currentCategory = await _unitOfWork.Categories.GetAsync(category => category.CategoryId == categoryId);

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
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Complete();

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
            var currentCategory = await _unitOfWork.Categories.GetAsync(category => category.CategoryId == categoryId);
            if (currentCategory is null)
                return NotFound();


            return View("Delete", currentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndSave(int categoryId)
        {
            var currentCategory = await _unitOfWork.Categories.GetAsync(category => category.CategoryId == categoryId);

            if (currentCategory is null)
                return NotFound();

            await _unitOfWork.Categories.Delete(currentCategory);
            _unitOfWork.Complete();

            // for notification purposes
            TempData["success"] = "Category Deleted Successfully!";

            return RedirectToAction("Index");
        }
    }
}
