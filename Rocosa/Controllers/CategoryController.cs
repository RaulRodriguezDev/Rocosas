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

        // Get
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Category.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }

            var category = _dbContext.Category.Find(id);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                _dbContext.Category.Update(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public IActionResult Delete(int? id)
        {
            if(id==0 ||id==null)
            {
                return NotFound();
            }

            var category = _dbContext.Category.Find(id);

            if(category == null )
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _dbContext.Remove(category);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
