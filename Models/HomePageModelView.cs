using BookShoppingCartMvcUI.Repo;

namespace BookShoppingCartMvcUI.Models
{
    public class HomePageModelView
    {
        public IEnumerable<Genre> GenreList { get; set; } = Enumerable.Empty<Genre>();
        public IEnumerable<Book> BookList { get; set; } = Enumerable.Empty<Book>();
        public int CartItemCount { get; set; }

        public HomePageModelView() { }
    
    }
}
