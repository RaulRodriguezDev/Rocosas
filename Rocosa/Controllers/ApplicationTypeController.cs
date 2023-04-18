using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocosa.Data;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Controllers
{
    [Authorize(Roles =WebConstants.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _applicationTypeRepository;

        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepository)
        {
           _applicationTypeRepository= applicationTypeRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypes = _applicationTypeRepository.GetAll();
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
            _applicationTypeRepository.Add(applicationType);
            _applicationTypeRepository.Record();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var applicationType = _applicationTypeRepository.GetById(id.GetValueOrDefault());

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
            _applicationTypeRepository.Update(applicationType);
            _applicationTypeRepository.Record();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }

            var applicationType = _applicationTypeRepository.GetById(id.GetValueOrDefault());

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
            _applicationTypeRepository.Remove(applicationType);
            _applicationTypeRepository.Record();

            return RedirectToAction("Index");
        }
    }
}
