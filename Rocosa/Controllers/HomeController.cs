using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Rocosa.Data;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using System.Diagnostics;

namespace Rocosa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = _dbContext.Products.Include(c => c.Category).Include(t => t.ApplicationType),
                Categories = _dbContext.Category
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int? id)
        {
            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = _dbContext.Products.Include(c => c.Category).Include(t => t.ApplicationType)
                                                .Where(p => p.Id == id).FirstOrDefault(),
                ExistInCarShop = false
            };

            return View(detailsViewModel);
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
    }
}