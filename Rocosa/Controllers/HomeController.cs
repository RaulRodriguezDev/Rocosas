using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Rocosa.Data;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Utilities;
using System.Diagnostics;

namespace Rocosa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext,
            ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _categoryRepository= categoryRepository;
            _productRepository= productRepository;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = _productRepository.GetAll(includeProperties: "Category,ApplicationType"),
                Categories = _categoryRepository.GetAll()
            };

            return View(homeViewModel);
        }

        public IActionResult Details(int? id)
        {
            List<ShopCart> shopCartItems = new List<ShopCart>();

            if (HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null
                && HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shopCartItems = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            DetailsViewModel detailsViewModel = new DetailsViewModel()
            {
                Product = _productRepository.GetFirst(p => p.Id == id,includeProperties: "Category,ApplicationType"),
                ExistInCarShop = false
            };

            foreach(var item in shopCartItems)
            {
                if(item.ProductId== id)
                {
                    detailsViewModel.ExistInCarShop = true;
                }
            }

            return View(detailsViewModel);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult Detials(int id, DetailsViewModel detailsViewModel)
        {
            List<ShopCart> shopCartItems= new List<ShopCart>();

            if(HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null 
                && HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shopCartItems = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }
            shopCartItems.Add(new ShopCart { ProductId = id, SquareMeter = detailsViewModel.Product.TempSquareMeter });
            HttpContext.Session.Set(WebConstants.CartShopSession, shopCartItems);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShopCart> shopCartItems = new List<ShopCart>();

            if (HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null
                && HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shopCartItems = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            var productToRemove = shopCartItems.SingleOrDefault(x => x.ProductId == id);

            if(productToRemove != null)
            {
                shopCartItems.Remove(productToRemove);
            }

            HttpContext.Session.Set(WebConstants.CartShopSession, shopCartItems);

            return RedirectToAction("Index");
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