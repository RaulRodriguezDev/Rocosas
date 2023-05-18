using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;
using System.Linq.Expressions;

namespace Rocosa.Data.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(Order order)
        {
            _context.Update(order);
        }
    }
}
