using Microsoft.AspNetCore.Mvc.Rendering;
using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetAll(string obj)
        {
            if(obj == WebConstants.CategoryName)
            {
                return _context.Category.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }

            if(obj == WebConstants.ApplicationTypeName)
            {
                return _context.ApplicationType.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
            }
            return null;
        }

        public void Update(Product product)
        {
            _context.Update(product);
        }
    }
}
