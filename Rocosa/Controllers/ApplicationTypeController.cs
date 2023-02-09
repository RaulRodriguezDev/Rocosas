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
    }
}
