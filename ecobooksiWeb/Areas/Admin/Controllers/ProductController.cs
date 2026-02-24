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
            private readonly IWebHostEnvironment _webHostEnvironment;

            public ProductController(IUnitOfWork unitOfWork, IProductRepository productRepository,
                IWebHostEnvironment webHostEnvironment)
            {
                _unitOfWork = unitOfWork;
                _productRepository = productRepository;
                _webHostEnvironment = webHostEnvironment;
            }

            // GET: ProductController
            [HttpGet]
            public IActionResult Index()
            {
                if(ModelState.IsValid)
                {
                    var products = _unitOfWork.Product.GetAll("Category");

                    return View(nameof(Index), products);
                }
                return NotFound();
            }

            //GET: ProductController/Details/5
            //[HttpGet("{productId}")] 
            public IActionResult Details(int productId)
            {
                var product = _unitOfWork.Product.Get(product => product.ProductId == productId,
                    "Category");

                if (product is null)
                    return NotFound();

                return View(nameof(Details), product);
            }

            [HttpGet]
            public IActionResult Upsert(int? productId)
            {
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

                if (productId is null || productId == 0)
                {
                    // Create new product
                    return View(viewModel);
                }
                else
                {
                    // Update existing product
                    // sending the product details to the view to pre-populate the form fields
                    viewModel.Product = _unitOfWork.Product
                        .Get(product => product.ProductId == productId);
                    
                    return View(viewModel);
                }

            }

            // GET: ProductController/Create
            //[HttpGet("CreateProduct")]
            //public IActionResult Create()
            //{
            //    //IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll()
            //    //    .Select(category => new SelectListItem
            //    //    {
            //    //        Value = category.CategoryId.ToString(),
            //    //        Text = category.CategoryName
            //    //    });

            //    //ViewBag.CategoryList = categoryList; 
            //    //ViewData["CategoryList"] = categoryList;

            //    ProductViewModel viewModel = new ProductViewModel()
            //    {
            //        Product = new Product(),
            //        CategoryList = _unitOfWork.Category.GetAll()
            //            .Select(category => new SelectListItem
            //            {
            //                Value = category.CategoryId.ToString(),
            //                Text = category.CategoryName
            //            })
            //    };

            //    return View(nameof(Create), viewModel);
            //}

            // POST: ProductController/Upsert
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Upsert(ProductViewModel productViewModel, IFormFile? file)
            {

                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    if (file is not null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var productPath = Path.Combine(wwwRootPath, @"images\product");

                        // in case of updating existing product, we need to check if there is an old image and delete it before saving the new one
                        // if old image exists, delete it
                        if (!string.IsNullOrEmpty(productViewModel.Product.ImageURL))
                        {
                            // delete the old image if it exists
                            var oldImagePath = Path.Combine(wwwRootPath, 
                                productViewModel.Product.ImageURL.TrimStart('\\', '/'));

                            if (System.IO.File.Exists(oldImagePath))
                                System.IO.File.Delete(oldImagePath);
                        };

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productViewModel.Product.ImageURL = @"\images\product\" + fileName;
                    }

                    if(productViewModel.Product.ProductId == 0)
                    {
                        await _productRepository.CreateAsync(productViewModel.Product);
                    }
                    else
                    {
                        _productRepository.Update(productViewModel.Product);
                    }

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
                           Text = category.CategoryName,
                           Value = category.CategoryId.ToString()
                       });
                
                    return View(productViewModel);
                }
            }

            //// GET: ProductController/Edit/5
            //[HttpGet("id")]
            //public async Task<IActionResult> Edit(int productId)
            //{
            //    if (productId is 0)
            //        return NotFound();

            //    var cuurentProduct = _unitOfWork.Product
            //        .Get(product => product.ProductId == productId);

            //    if(cuurentProduct is null)
            //        return NotFound();

            //    return View(nameof(Edit), cuurentProduct);
            //}

            // POST: ProductController/Edit/5
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public IActionResult EditAndSave(Product product)
            //{
            //    if (product is null)
            //        return NotFound();

            //    if (ModelState.IsValid)
            //    {
            //        _unitOfWork.Product.Update(product);
            //        _unitOfWork.Complete();

            //        TempData["success"] = "Product Updated Successfully!";

            //        return RedirectToAction(nameof(Index));
            //    }

            //    return View(nameof(Edit), product);
            //}

            //// GET: ProductController/Delete/5
            //[HttpGet("DeleteProduct")]
            //public IActionResult Delete(int productId)
            //{
            //    if(productId is 0)
            //        return NotFound();

            //    var currentProduct = _unitOfWork.Product
            //        .Get(product => product.ProductId == productId);

            //    if(currentProduct is null)
            //        return NotFound();  

            //    return View(nameof(Delete), currentProduct);
            //}

            //// POST: ProductController/Delete/5
            //[HttpPost]
            //[ValidateAntiForgeryToken]
            //public async Task<IActionResult> DeleteAndSave(int productId)
            //{
            //    if(productId is 0)
            //        return NotFound();

            //    if (ModelState.IsValid)
            //    {
            //        var currentProduct = _unitOfWork.Product
            //            .Get(product => product.ProductId == productId);

            //        if(currentProduct is null)
            //            return NotFound();

            //        await _unitOfWork.Product.DeleteAsync(currentProduct);
            //        _unitOfWork.Complete();

            //        TempData["success"] = "Product Deleted Successfully!";

            //        return RedirectToAction(nameof(Index));
            //    }
            //    return View(nameof(Delete));
            //}

            #region API Calls

            [HttpGet]
            public IActionResult GetAll()
            {
                var products = _unitOfWork.Product.GetAll("Category");
                return Json(new { data = products });
            }

            [HttpDelete]
            public IActionResult Delete(int productId)
            {
                var currentProduct = _unitOfWork.Product
                    .Get(product => product.ProductId == productId);

                if(currentProduct is null)
                    return Json(new { success = false, message = "Error while deleting" });

                // delete the image file from the wwwroot folder if it exists
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var oldImagePath = Path.Combine(wwwRootPath,
                                currentProduct.ImageURL.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.DeleteAsync(currentProduct);
                _unitOfWork.Complete();

                return Json(new { success = true, message = "Product Deleted Successfully!" });
            }
            #endregion
        }
    }
