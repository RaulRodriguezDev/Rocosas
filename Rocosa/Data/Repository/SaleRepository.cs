using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(Sale sale)
        {
            _context.Update(sale);
        }
    }
}
