using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface ISaleDetailsRepository: IRepository<SaleDetails>
    {
        void Update(SaleDetails saleDetails);
    }
}
