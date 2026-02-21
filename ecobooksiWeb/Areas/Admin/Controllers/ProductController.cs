using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecobooksi.Models.View_Models;

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
        public IActionResult Index()
        {
            if(ModelState.IsValid)
            {
                var products = _unitOfWork.Product.GetAll();

                return View(nameof(Index), products);
            }
            return NotFound();
        }

        // GET: ProductController/Details/5
        [HttpGet("ProductDetils/{productId:int}")]
        public async Task<IActionResult> Details(int productId)
        {
            var product = await _unitOfWork.Product.GetAsync(product => product.ProductId == productId);
            
            if(product is null)
                return NotFound();

            return View(nameof(Details), product);
        }

        // GET: ProductController/Create
        [HttpGet("CreateProduct")]
        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
            //    .Select(category => new SelectListItem
            //    {
            //        Value = category.CategoryId.ToString(),
            //        Text = category.CategoryName
            //    });

            //ViewBag.CategoryList = categoryList; 
            //ViewData["CategoryList"] = categoryList;

            ProductViewModel viewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll()
                    .Select(category => new SelectListItem
                    {
                        Value = category.CategoryId.ToString(),
                        Text = category.CategoryName
                    })
            };

            return View(nameof(Create), viewModel);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAndSave(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Product.CreateAsync(productViewModel.Product);
                _unitOfWork.Complete();

                TempData["success"] = "Product Created Successfully!";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // when the model state is invalid,
                // we need to repopulate the CategoryList for the view to work properly
                productViewModel.CategoryList = _unitOfWork.Category.GetAll()
                   .Select(category => new SelectListItem
                   {
                       Value = category.CategoryId.ToString(),
                       Text = category.CategoryName
                   });
                
                return View(nameof(Create), productViewModel);
            }
        }

        // GET: ProductController/Edit/5
        [HttpGet("id")]
        public async Task<IActionResult> Edit(int productId)
        {
            if (productId is 0)
                return NotFound();

            var cuurentProduct = await _unitOfWork.Product
                .GetAsync(product => product.ProductId == productId);

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
        public async Task<IActionResult> Delete(int productId)
        {
            if(productId is 0)
                return NotFound();

            var currentProduct = await _unitOfWork.Product
                .GetAsync(product => product.ProductId == productId);

            if(currentProduct is null)
                return NotFound();  

            return View(nameof(Delete), currentProduct);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAndSave(int productId)
        {
            if(productId is 0)
                return NotFound();

            if (ModelState.IsValid)
            {
                var currentProduct = await _unitOfWork.Product
                    .GetAsync(product => product.ProductId == productId);

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
