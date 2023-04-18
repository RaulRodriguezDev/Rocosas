using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var previousCategory = _context.Category.FirstOrDefault(c => c.Id == category.Id);

            if (previousCategory != null)
            {
                previousCategory.Name= category.Name;
                previousCategory.ShowOrder = category.ShowOrder;
            }
        }
    }
}
