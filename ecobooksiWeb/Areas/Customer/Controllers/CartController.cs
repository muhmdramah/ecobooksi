using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using ecobooksi.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecobooksi.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

        public CartController(IUnitOfWork unitOfWork, IShoppingCartRepository shoppingCartRepository)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartRepository = shoppingCartRepository;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartViewModel = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart
                    .GetAll(user => user.ApplicationUserId == userId, "Product"),
            };

            foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);

                ShoppingCartViewModel.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartViewModel);
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var currentCart = _unitOfWork.ShoppingCart
                .Get(filter: cart => cart.ShoppingCartId == cartId);

            currentCart.Count += 1;

            _unitOfWork.ShoppingCart.Update(currentCart);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var currentCart = _unitOfWork.ShoppingCart
                 .Get(filter: cart => cart.ShoppingCartId == cartId);

            if (currentCart.Count <= 1)
            {
                await _unitOfWork.ShoppingCart.DeleteAsync(currentCart);
            }
            else
            {
                currentCart.Count -= 1;
                _unitOfWork.ShoppingCart.Update(currentCart);
                _unitOfWork.Complete();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var currentCart = _unitOfWork.ShoppingCart
                 .Get(filter: cart => cart.ShoppingCartId == cartId);

            await _unitOfWork.ShoppingCart.DeleteAsync(currentCart);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.PriceFifty;
                }
                else
                {
                    return shoppingCart.Product.PriceHundred;
                }
            }
        }
    }

}
