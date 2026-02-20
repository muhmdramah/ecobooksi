using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecobooksi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductController(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        // GET: ProductController
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(ModelState.IsValid)
            {
                var products = await  _unitOfWork.Product.GetAllAsync();
                return View(nameof(Index), products);
            }
            return NotFound();
        }

        // GET: ProductController/Details/5
        [HttpGet("ProductDetils/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _unitOfWork.Product.GetAsync(product => product.ProductId == id);
            
            if(product is null)
                return NotFound();

            return View(nameof(Details), product);
        }

        // GET: ProductController/Create
        [HttpGet("CreateProduct")]
        public IActionResult Create()
        {
            return View(nameof(Create));
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndSave(Product product)
        {
            if (product.Title is null)
                ModelState.AddModelError("Title", "Product title must be not null!");
            
            if (ModelState.IsValid)
            {
                await _unitOfWork.Product.CreateAsync(product);
                _unitOfWork.Complete();

                TempData["success"] = "Product Created Successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Create), product);
        }

        // GET: ProductController/Edit/5
        [HttpGet("id")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id is 0)
                return NotFound();

            var cuurentProduct = await _unitOfWork.Product
                .GetAsync(product => product.ProductId == id);

            if(cuurentProduct is null)
                return NotFound();

            return View(nameof(Edit), cuurentProduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAndSave(Product product)
        {
            if (product is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Complete();

                TempData["success"] = "Product Updated Successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(Edit), product);
        }

        // GET: ProductController/Delete/5
        [HttpGet("DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id is 0)
                return NotFound();

            var currentProduct = await _unitOfWork.Product
                .GetAsync(product => product.ProductId == id);

            if(currentProduct is null)
                return NotFound();  

            return View(nameof(Delete), currentProduct);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndSave(int id)
        {
            if(id is 0)
                return NotFound();

            if (ModelState.IsValid)
            {
                var currentProduct = await _unitOfWork.Product
                    .GetAsync(product => product.ProductId == id);

                if(currentProduct is null)
                    return NotFound();

                await _unitOfWork.Product.DeleteAsync(currentProduct);
                _unitOfWork.Complete();

                TempData["success"] = "Product Deleted Successfully!";

                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Delete));
        }
    }
}
