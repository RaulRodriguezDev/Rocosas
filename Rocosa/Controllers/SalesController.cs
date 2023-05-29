using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models.ViewModels;

namespace Rocosa.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleDetailsRepository _saleDetailsRepository;

        public SalesController(ISaleRepository saleRepository, ISaleDetailsRepository saleDetailsRepository)
        {
            _saleRepository = saleRepository;
            _saleDetailsRepository = saleDetailsRepository;
        }
        public IActionResult Index(string searchName = null, string searchPhone = null, string searchEmail = null, string Status = null)
        {
            SaleViewModel saleViewModel = new SaleViewModel()
            {
                SalesList= _saleRepository.GetAll(),
                StatusList = WebConstants.StatusList.ToList().Select(l => new SelectListItem
                {
                    Text = l,
                    Value = l
                })
            };

            if(!string.IsNullOrEmpty(searchName) )
            {
                saleViewModel.SalesList = saleViewModel.SalesList.Where(s => s.FullName.ToLower()
                .Contains(searchName.ToLower())); 
            }
            
            if(!string.IsNullOrEmpty(searchEmail) )
            {
                saleViewModel.SalesList = saleViewModel.SalesList.Where(s => s.Email.ToLower()
                .Contains(searchEmail.ToLower())); 
            }            
            
            if(!string.IsNullOrEmpty(searchPhone))
            {
                saleViewModel.SalesList = saleViewModel.SalesList.Where(s => s.Phone.ToLower()
                .Contains(searchPhone.ToLower())); 
            }

            if (!string.IsNullOrEmpty(Status) && Status != "--Status--")
            {
                saleViewModel.SalesList = saleViewModel.SalesList.Where(s => s.SaleStatus== Status);
            }
            return View(saleViewModel);
        }
    }
}
