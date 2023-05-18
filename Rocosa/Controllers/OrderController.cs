using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using Rocosa.Utilities;

namespace Rocosa.Controllers
{
    [Authorize(Roles =WebConstants.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;

        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id) 
        {
            OrderViewModel = new OrderViewModel()
            {
                Order = _orderRepository.GetFirst(o => o.Id == id),
                OrderDetail = _orderDetailRepository.GetAll(d => d.Id == id, includeProperties: "Product")
            };

            return View(OrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail()
        {
            List<ShopCart> shopCartList = new List<ShopCart>();
            OrderViewModel.OrderDetail = _orderDetailRepository.GetAll(d => d.OrderId == OrderViewModel.Order.Id);

            foreach(var detail in OrderViewModel.OrderDetail) 
            {
                ShopCart shopCart = new ShopCart() 
                {
                    ProductId= detail.ProductId
                };
                shopCartList.Add(shopCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WebConstants.CartShopSession, shopCartList);
            HttpContext.Session.Set("SessionOrder", OrderViewModel.Order.Id);
            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            Order order = _orderRepository.GetFirst(o => o.Id == OrderViewModel.Order.Id);
            IEnumerable<OrderDetail> orderDetail = _orderDetailRepository.GetAll(d => d.Id == OrderViewModel.Order.Id);

            _orderDetailRepository.RemoveRange(orderDetail);
            _orderRepository.Remove(order);
            _orderRepository.Record();
            _orderDetailRepository.Record();

            return RedirectToAction(nameof(Index));
        }
        #region APIS

        [HttpGet] 
        public IActionResult GetOrderList() 
        {
            return Json(new { data = _orderRepository.GetAll() });
        }
        #endregion
    }
}
