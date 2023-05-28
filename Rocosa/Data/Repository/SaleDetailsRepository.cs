using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class SaleDetailsRepository : Repository<SaleDetails>, ISaleDetailsRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleDetailsRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(SaleDetails saleDetails)
        {
            _context.Update(saleDetails);
        }
    }
}
