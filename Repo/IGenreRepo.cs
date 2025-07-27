using BookShoppingCartMvcUI.Data;
using BookShoppingCartMvcUI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repo
{
    public interface IGenreRepo
    {


        public  Task AddGenre(Genre genre);


        public  Task UpdateGenre(Genre genre);



        public  Task DeleteGenre(Genre genre);



        public Task<Genre?> GetGenreById(int id);


        public Task<IEnumerable<Genre>> GetGenres();
      
    }
}
