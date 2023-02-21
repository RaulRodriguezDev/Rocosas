
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Utilities;
using System.Security.Claims;

namespace Rocosa.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        [BindProperty]
        public UserProductViewModel UserProductViewModel { get; set; }

        public ShoppingCartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<ShopCart> shoppingCartList = new List<ShopCart>();

            if(HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) !=null &&
                HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count()>0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            List<int> prodcutsListInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = _dbContext.Products.Where(p => prodcutsListInCart.Contains(p.Id));

            return View(productsList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction("Overview");
        }

        public IActionResult Overview()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<ShopCart> shoppingCartList = new List<ShopCart>();

            if (HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null &&
                HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            List<int> prodcutsListInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = _dbContext.Products.Where(p => prodcutsListInCart.Contains(p.Id));

            UserProductViewModel = new UserProductViewModel()
            {
                UserApplication = _dbContext.UserApplications.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = productsList
            };

            return View(UserProductViewModel);

        }

        public IActionResult Remove(int Id)
        {
            List<ShopCart> shoppingCartList = new List<ShopCart>();

            if (HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null &&
                HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(p => p.ProductId== Id));
            HttpContext.Session.Set(WebConstants.CartShopSession, shoppingCartList);

            return RedirectToAction("Index");
        }
    }
}
