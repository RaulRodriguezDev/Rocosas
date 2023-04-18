using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using System.Collections;

namespace Rocosa.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository= categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.GetAll();

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
                _categoryRepository.Add(category);
                _categoryRepository.Record();
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

            var category = _categoryRepository.GetById(id.GetValueOrDefault());

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
                _categoryRepository.Update(category);
                _categoryRepository.Record();
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

            var category = _categoryRepository.GetById(id.GetValueOrDefault());

            if(category == null )
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            _categoryRepository.Remove(category);
            _categoryRepository.Record();

            return RedirectToAction("Index");
        }
    }
}
