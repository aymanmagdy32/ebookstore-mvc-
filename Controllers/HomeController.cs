using BookShoppingCartMvcUI.Models;
using BookShoppingCartMvcUI.Repo;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace BookShoppingCartMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepo _homeRepo;
        private readonly ICartRepo _CartRepo;
        public HomeController(ILogger<HomeController> logger , IHomeRepo homeRepo , ICartRepo cartRepo)
        {
            _logger = logger;
            _homeRepo = homeRepo;
            _CartRepo = cartRepo; 
        }

        public async Task<IActionResult> Index()
        {
            // This will get all books (no filters)
            var model = new HomePageModelView
            {
                BookList = await _homeRepo.GetBooks(),
                GenreList = await _homeRepo.GetGenra(),

            };

            return View("Index", model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public async Task<IActionResult> SearchDisplay(string name, int genre = 0)
        {
            var model = new HomePageModelView
            {
                BookList = await _homeRepo.GetBooks(name, genre),
                 GenreList = await _homeRepo.GetGenra() // d\

            };  

            return View("Index", model);
        }


        public async Task<IActionResult> GetGenre()
        {
            var model = new HomePageModelView
            {
                GenreList = await _homeRepo.GetGenra()
            };  

            return View("Index", model);
        }


    }
}
