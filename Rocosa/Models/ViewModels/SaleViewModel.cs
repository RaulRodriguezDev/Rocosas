using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocosa.Models.ViewModels
{
    public class SaleViewModel
    {
        public IEnumerable<Sale> SalesList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string Status { get; set; }
    }
}
