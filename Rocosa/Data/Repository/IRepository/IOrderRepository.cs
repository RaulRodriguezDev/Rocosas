using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface IOrderRepository: IRepository<Order>
    {
        void Update(Order order);
    }
}
