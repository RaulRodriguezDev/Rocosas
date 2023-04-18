using Microsoft.AspNetCore.Mvc.Rendering;
using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        IEnumerable<SelectListItem> GetAll(string obj);
    }
}
