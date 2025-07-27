using BookShoppingCartMvcUI.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMvcUI.Controllers
{
    [Authorize]
    public class UserOrderController : Controller 
    {
        private readonly IUserOrderRepo _userOrderRepo;
        public UserOrderController(IUserOrderRepo userOrderRepo)
        {
            _userOrderRepo = userOrderRepo; 
        }

        public async Task  <IActionResult> UserOrders()
        {
            var orders = await _userOrderRepo.UserOrders(); 
         return View("UserOrders" , orders);
        }

    }
}
