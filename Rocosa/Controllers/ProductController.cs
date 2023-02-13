using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Data;
using Rocosa.Models;
using Rocosa.Models.ViewModels;

namespace Rocosa.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _dbContext.Products.Include(c=>c.Category).Include(a=>a.ApplicationType);
            return View(products);
        }

        // Get
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categoriesDropDown = _dbContext.Category.Select(c => new SelectListItem
            //{
            //    Text = c.Name,
            //    Value = c.Id.ToString(),
            //});

            //ViewBag.Categories = categoriesDropDown;

            //Product product = new Product();

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                Categories = _dbContext.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                ApplicationTypeList = _dbContext.ApplicationType.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if(id==null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product =_dbContext.Products.Find(id);

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }

                return View(productViewModel);
            }
        }
    }
}
