using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface ISaleRepository: IRepository<Sale>
    {
        void Update(Sale sale);
    }
}
