using BookShoppingCartMvcUI.Models;

namespace BookShoppingCartMvcUI
{
    public interface IHomeRepo
    {

        Task<IEnumerable<Book>> GetBooks();
        Task<IEnumerable<Book>> GetBooks(string sTerm, int categoryId = 0);
        Task<IEnumerable<Genre>> GetGenra();
    
    }
}