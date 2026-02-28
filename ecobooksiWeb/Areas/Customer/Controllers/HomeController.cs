using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ecobooksi.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll("Category");
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitOfWork.Product
                    .Get(product => product.ProductId == productId, "Category"),
                
                Count = 1,
                ProductId = productId
            };

            return View(shoppingCart);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            var currentCart = _unitOfWork.ShoppingCart
                    .Get(cart => cart.ApplicationUserId == shoppingCart.ApplicationUserId
                        && cart.ProductId == shoppingCart.ProductId);

            if(currentCart is not null)
            {
                currentCart.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(currentCart);
                _unitOfWork.Complete();
            }
            else
            {
                await _unitOfWork.ShoppingCart.CreateAsync(shoppingCart);
                _unitOfWork.Complete();
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
