
using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Services.BrainTree;
using Rocosa.Utilities;
using System.Security.Claims;
using System.Text;

namespace Rocosa.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IProductRepository _productRepository;
        private readonly IUserApplicationRepository _userApplicationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ISaleDetailsRepository _saleDetailsRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IBrainTreeGates _brainTreeGates;

        [BindProperty]
        public UserProductViewModel UserProductViewModel { get; set; }

        public ShoppingCartController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, 
            IEmailSender emailSender, IUserApplicationRepository userApplicationRepository, 
            IProductRepository productRepository, IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ISaleRepository saleRepository,
            ISaleDetailsRepository saleDetailsRepository,
            IBrainTreeGates brainTreeGates)
        {
            _orderRepository= orderRepository;
            _orderDetailRepository= orderDetailRepository;
            _userApplicationRepository = userApplicationRepository;
            _productRepository= productRepository;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _saleDetailsRepository= saleDetailsRepository;
            _saleRepository= saleRepository;
            _brainTreeGates= brainTreeGates;
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
            IEnumerable<Product> productsList = _productRepository.GetAll(p => prodcutsListInCart.Contains(p.Id));/*_dbContext.Products.Where(p => prodcutsListInCart.Contains(p.Id));*/
            List<Product> products = new List<Product>();

            foreach(var objCart in shoppingCartList)
            {
                Product productTemp = productsList.FirstOrDefault(p => p.Id == objCart.ProductId);
                productTemp.TempSquareMeter = objCart.SquareMeter;
                products.Add(productTemp);
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost(IEnumerable<Product> products)
        {
            List<ShopCart> shoppingCartList = new List<ShopCart>();

            foreach (Product product in products)
            {
                shoppingCartList.Add(new ShopCart
                {
                    ProductId = product.Id,
                    SquareMeter = product.TempSquareMeter
                });
            }
            HttpContext.Session.Set(WebConstants.CartShopSession, shoppingCartList);

            return RedirectToAction("Overview");
        }

        public IActionResult Overview()
        {
            UserApplication userApplication;

            if (User.IsInRole(WebConstants.AdminRole))
            {
                if(HttpContext.Session.Get<int>(WebConstants.SessionOrderId) != 0)
                {
                    Order order = _orderRepository.GetFirst(o => o.Id ==
                    HttpContext.Session.Get<int>(WebConstants.SessionOrderId));
                    userApplication = new UserApplication()
                    {
                        Email = order.Email,
                        FullName = order.FullName,
                        PhoneNumber = order.Phone
                    };
                }

                else
                {
                    userApplication = new UserApplication();
                }

                var gateway = _brainTreeGates.GetGateWay();
                var clientToken = gateway.ClientToken.Generate();
                ViewBag.ClientToken = clientToken;
            }

            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userApplication = _userApplicationRepository.GetFirst(u => u.Id == claim.Value);

            }

            List<ShopCart> shoppingCartList = new List<ShopCart>();

            if (HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession) != null &&
                HttpContext.Session.Get<IEnumerable<ShopCart>>(WebConstants.CartShopSession).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShopCart>>(WebConstants.CartShopSession);
            }

            List<int> prodcutsListInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> productsList = _productRepository.GetAll(p => prodcutsListInCart.Contains(p.Id));

            UserProductViewModel = new UserProductViewModel()
            {
                UserApplication = userApplication
            };

            foreach(var cart in shoppingCartList)
            {
                Product productTemp = _productRepository.GetFirst(p => p.Id == cart.ProductId);
                productTemp.TempSquareMeter = cart.SquareMeter;
                UserProductViewModel.ProductList.Add(productTemp);
            }
            return View(UserProductViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Overview(IFormCollection collection, UserProductViewModel userProductViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (User.IsInRole(WebConstants.AdminRole))
            {
                Sale sale = new Sale()
                {
                    CreatedByUserId = claim.Value,
                    FinalTotalSale = userProductViewModel.ProductList.Sum(p => p.TempSquareMeter * p.ProductPrice),
                    Address = userProductViewModel.UserApplication.Address,
                    City= userProductViewModel.UserApplication.City,
                    Phone = userProductViewModel.UserApplication.PhoneNumber,
                    FullName = userProductViewModel.UserApplication.FullName,
                    Email= userProductViewModel.UserApplication.Email,
                    SaleDate = DateTime.Now,
                    SaleStatus = WebConstants.PendingStatus,
                    TransactionId = ""
                };

                _saleRepository.Add(sale);
                _saleRepository.Record();

                foreach (var product in userProductViewModel.ProductList)
                {
                    SaleDetails saleDetails = new SaleDetails()
                    {
                        SaleId = sale.Id,
                        PriceBySquareMeter = product.ProductPrice,
                        SquareMeter = product.TempSquareMeter,
                        ProductId = product.Id
                    };

                    _saleDetailsRepository.Add(saleDetails);
                }
                _saleDetailsRepository.Record();

                string nonceFromTheClient = collection["payment_method_nonce"];
                var gateway = _brainTreeGates.GetGateWay();
                var request = new TransactionRequest
                {
                    Amount = Convert.ToDecimal(sale.FinalTotalSale),
                    PaymentMethodNonce = nonceFromTheClient,
                    OrderId = sale.Id.ToString(),
                    Options = new TransactionOptionsRequest
                    {
                        SubmitForSettlement = true
                    }
                };

                Result<Transaction> result = gateway.Transaction.Sale(request);

                if(result.Target.ProcessorResponseText == "Approved")
                {
                    sale.TransactionId = result.Target.Id;
                    sale.SaleStatus = WebConstants.ApprovedStatus;
                }

                else
                {
                    sale.SaleStatus = WebConstants.CanceledStatus;
                }

                _saleRepository.Record();
                return RedirectToAction("Confirm", new {id = sale.Id});
            }

            else
            {
                var templateRoute = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                + "templates" + Path.DirectorySeparatorChar.ToString() + "PlantillaOrden.html";

                var subject = "New Order";
                string htmlBody = "";

                using (StreamReader streamReader = System.IO.File.OpenText(templateRoute))
                {
                    htmlBody = streamReader.ReadToEnd();
                }
                StringBuilder productListSb = new StringBuilder();

                foreach (var product in userProductViewModel.ProductList)
                {
                    productListSb.Append($" - Name: {product.Name} <span style='font-size:14px;'> (ID: {product.Id})</span><br/>");
                }

                string messageBody = String.Format(htmlBody,
                    userProductViewModel.UserApplication.FullName,
                    userProductViewModel.UserApplication.Email,
                    userProductViewModel.UserApplication.PhoneNumber,
                    productListSb.ToString());

                await _emailSender.SendEmailAsync("", subject, messageBody);

                Order order = new Order()
                {
                    UserApplicationId = claim.Value,
                    FullName = userProductViewModel.UserApplication.FullName,
                    Email = userProductViewModel.UserApplication.Email,
                    Phone = userProductViewModel.UserApplication.PhoneNumber,
                    OrderDate = DateTime.Now
                };

                _orderRepository.Add(order);
                _orderRepository.Record();

                foreach (var product in userProductViewModel.ProductList)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        ProductId = product.Id
                    };
                    _orderDetailRepository.Add(orderDetail);
                }
                _orderDetailRepository.Record();

            }
             
           return RedirectToAction("Confirm");
        }

        public IActionResult Confirm(int id = 0)
        {
            Sale sale = _saleRepository.GetFirst(s => s.Id== id);
            HttpContext.Session.Clear();
            return View(sale);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateShoppingCart(IEnumerable<Product> products)
        {
            List<ShopCart> shoppingCartList = new List<ShopCart>();

            foreach (Product product in products) 
            {
                shoppingCartList.Add(new ShopCart
                {
                    ProductId = product.Id,
                    SquareMeter = product.TempSquareMeter
                });
            }
            HttpContext.Session.Set(WebConstants.CartShopSession, shoppingCartList);

            return RedirectToAction("Index");
        }

        public IActionResult Clean()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }
    }
}
