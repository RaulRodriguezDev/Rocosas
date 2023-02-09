using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Models;

namespace Rocosa.Controllers
{
    public class ApplicationTypeController : Controller
    {
        public ApplicationDbContext _dbContext;

        public ApplicationTypeController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypes = _dbContext.ApplicationType;
            return View(applicationTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            _dbContext.ApplicationType.Add(applicationType);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var applicationType = _dbContext.ApplicationType.Find(id);

            if (applicationType == null)
            {
                return NotFound();
            }

            return View(applicationType);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            _dbContext.ApplicationType.Update(applicationType);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }

            var applicationType = _dbContext.ApplicationType.Find(id);

            if(applicationType == null)
            {
                return NotFound();
            }

            return View(applicationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationType applicationType)
        {
            _dbContext.ApplicationType.Remove(applicationType);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
