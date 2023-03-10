
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Utilities;
using System.Security.Claims;
using System.Text;

namespace Rocosa.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public UserProductViewModel UserProductViewModel { get; set; }

        public ShoppingCartController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
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
                ProductList = productsList.ToList()
            };

            return View(UserProductViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Overview(UserProductViewModel userProductViewModel)
        {
            var templateRoute = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() + "PlantillaOrden.html";

            var subject = "New Order";
            string htmlBody = "";

            using(StreamReader streamReader = System.IO.File.OpenText(templateRoute))
            {
                htmlBody = streamReader.ReadToEnd();
            }
            StringBuilder productListSb = new StringBuilder();

            foreach(var product in userProductViewModel.ProductList)
            {
                productListSb.Append($" - Name: {product.Name} <span style='font-size:14px;'> (ID: {product.Id})</span><br/>");
            }

            string messageBody = String.Format(htmlBody,
                userProductViewModel.UserApplication.FullName,
                userProductViewModel.UserApplication.Email,
                userProductViewModel.UserApplication.PhoneNumber,
                productListSb.ToString());

           await _emailSender.SendEmailAsync("",subject,messageBody);
           return RedirectToAction("Confirm");
        }

        public IActionResult Confirm()
        {
            HttpContext.Session.Clear();
            return View();
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
