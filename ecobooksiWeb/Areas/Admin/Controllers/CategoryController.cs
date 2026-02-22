using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecobooksi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
        public IActionResult Index()
        {
            var categoris = _unitOfWork.Category.GetAll();

            if (ModelState.IsValid)
            {
                return View(nameof(Index), categoris);
            }

            return NotFound(); 
            // or return View(nameof(Index), new List<Category>()); to show empty list instead of 404 page
        }

        [HttpGet("CategoryDetails/{categoryId:int}")]
        public IActionResult Details(int categoryId)
        {
            var category = _unitOfWork.Category.Get(category => category.CategoryId == categoryId);

            if (ModelState.IsValid)
            {
                return View(nameof(Details), category);
            }

            return NotFound();
        }

        [HttpGet("CreateCategory")]
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndSave(Category category)
        {
            if (category.CategoryName == category.DisplayOrder.ToString())
                ModelState.AddModelError("CategoryName", "Display Order cannot exactly match the Category Name!");

            if (ModelState.IsValid)
            {
                await _unitOfWork.Category.CreateAsync(category);
                _unitOfWork.Complete();

                // for notification purposes
                TempData["success"] = "Category Created Successfully!";

                return RedirectToAction(nameof(Index), "Category");
            }

            return View(nameof(Create));
        }


        [HttpGet("EditCategory")]
        public IActionResult Edit(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();

            // send the currrent category with the opend view 
            var currentCategory =  _unitOfWork.Category.Get(category => category.CategoryId == categoryId);

            if (currentCategory is null)
                return NotFound();

            return View(nameof(Edit), currentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAndSave(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();

                // for notification purposes
                TempData["success"] = "Category Updated Successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Edit), category);
        }


        [HttpGet("DeleteCategory")]
        public IActionResult Delete(int categoryId)
        {
            if (categoryId == 0)
                return NotFound();

            // send the currrent category with the opend view 
            var currentCategory = _unitOfWork.Category.Get(category => category.CategoryId == categoryId);
            if (currentCategory is null)
                return NotFound();


            return View(nameof(Delete), currentCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndSave(int categoryId)
        {
            var currentCategory = _unitOfWork.Category.Get(category => category.CategoryId == categoryId);

            if (currentCategory is null)
                return NotFound();

            await _unitOfWork.Category.DeleteAsync(currentCategory);
            _unitOfWork.Complete();

            // for notification purposes
            TempData["success"] = "Category Deleted Successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
