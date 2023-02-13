using Microsoft.AspNetCore.Mvc.Rendering;

namespace Rocosa.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeList { get; set; }
    }
}
