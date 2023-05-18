using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.Update(orderDetail);
        }
    }
}
