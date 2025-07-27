using BookShoppingCartMvcUI.Data;
using BookShoppingCartMvcUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repo
{
    public class BookRepo : IBookRepo 
    {


        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookRepo(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<Book> GetBook(int BookId)
        {
            var book = _db.Books.FirstOrDefault(a => a.Id == BookId);

            return book;
        }


        public async Task AddBook(Book book )
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();

        }

        public async Task UpdateBook(Book book)
        {
            _db.Books.Update(book);
            await _db.SaveChangesAsync();
        }
      

        public async Task<Book?> GetBookById(int id) => await _db.Books.FindAsync(id);

        public async Task<IEnumerable<Book>> GetBooks() => await _db.Books.Include(a => a.Genre).ToListAsync();   // 

    }
}
