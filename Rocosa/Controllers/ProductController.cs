using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocosa.Data;
using Rocosa.Models;

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
            Product product = new Product();

            if(id==null)
            {
                return View(product);
            }
            else
            {
                product=_dbContext.Products.Find(id);

                if(product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
        }
    }
}
