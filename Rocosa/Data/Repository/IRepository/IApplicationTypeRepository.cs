using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface IApplicationTypeRepository : IRepository<ApplicationType>
    {
        void Update(ApplicationType applicationType);
    }
}
