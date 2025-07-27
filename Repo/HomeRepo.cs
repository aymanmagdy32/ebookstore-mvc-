using BookShoppingCartMvcUI.Data;
using BookShoppingCartMvcUI.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BookShoppingCartMvcUI.Repo
{
    public class HomeRepo: IHomeRepo
    {
       private readonly ApplicationDbContext _context;
        public HomeRepo(ApplicationDbContext db)
        {
            this._context = db; 
        }


        public async Task<IEnumerable<Genre>> GetGenra()
        {
            return await _context.Genres.ToListAsync();
        }







        public async Task<IEnumerable<Book>> GetBooks()
        {
            IEnumerable<Book> books = await (
                 from book in _context.Books
                 join genre in _context.Genres on book.GenreId equals genre.Id
            select new Book
            {
                Id = book.Id,
                     BookName = book.BookName,
                     AuthorName = book.AuthorName,
                     Price = book.Price,
                     Image = book.Image,
                     GenreName = genre.GenreName

                 }

                          ).ToListAsync();

            return books;

        }




          public async Task<IEnumerable<Book>> GetBooks(string sTerm="" , int categoryId = 0) 
        {
            sTerm = sTerm?.ToLower() ?? "";
            IEnumerable<Book> books = await (
                from book in _context.Books
                         join genre in _context.Genres on book.GenreId equals genre.Id
                               where ((string.IsNullOrEmpty(sTerm) || book.BookName.ToLower().Contains(sTerm)) && (categoryId == 0 || book.GenreId == categoryId) )
                select new Book
                         {
                             Id = book.Id,
                             BookName = book.BookName,
                             AuthorName = book.AuthorName,
                             Price = book.Price,
                             Image = book.Image,
                             GenreName = genre.GenreName

                         }

                         ).ToListAsync();

            return books; 
        
        }


      



    }
}
