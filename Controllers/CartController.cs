using BookShoppingCartMvcUI.Models;
using BookShoppingCartMvcUI.Models;
using BookShoppingCartMvcUI.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize]
    public class CartController : Controller 
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICartRepo _CartRepo;

        public CartController(ILogger<HomeController> logger, ICartRepo CartRepo)
        {
            _logger = logger;
            _CartRepo = CartRepo  ;
        }




        public async Task<IActionResult> AddItem(int bookId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _CartRepo.AddItem(bookId, qty);

            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }



        public async Task<IActionResult> RemoveItem(int bookId) 
        {
             await _CartRepo.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");

        }

        public async Task<IActionResult> GetUserCart() 
        {
            var cart = await _CartRepo.GetUserCart();
            return View("GetUserCart", cart);
        }


        public async Task<IActionResult> GetTotalItemsOnCart() 
        {

             int cartItem = await _CartRepo.GetCartItemCount();
             return Ok(cartItem); 
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _CartRepo.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

    }



}
