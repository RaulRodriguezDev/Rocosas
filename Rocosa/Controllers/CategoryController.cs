using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Models;
using System.Collections;

namespace Rocosa.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _dbContext.Category;

            return View(categories);
        }
    }
}
