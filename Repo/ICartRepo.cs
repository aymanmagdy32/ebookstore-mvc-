using BookShoppingCartMvcUI.Models;
using BookShoppingCartMvcUI.Models;

namespace BookShoppingCartMvcUI.Repo
{
     public interface ICartRepo
    {
        public Task<string> GetUserAsync();
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetUserCart();
        public Task<bool> DoCheckout(CheckoutModel model);

    }
}
