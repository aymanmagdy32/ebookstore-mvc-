using BookShoppingCartMvcUI.Models;

namespace BookShoppingCartMvcUI.Repo
{
    public interface IBookRepo
    {

        public Task<Book> GetBook(int BookId);


        public Task AddBook(Book book);


        public Task DeleteBook(Book book);


        public  Task UpdateBook(Book book);

        public  Task<Book?> GetBookById(int id);

        public Task<IEnumerable<Book>> GetBooks(); 

    }
}
