namespace Rocosa.Models.ViewModels
{
    public class UserProductViewModel
    {
        public UserProductViewModel()
        {
            ProductList= new List<Product>();
        }
        public UserApplication UserApplication { get; set; }

        public IList<Product> ProductList { get; set; }
    }
}
