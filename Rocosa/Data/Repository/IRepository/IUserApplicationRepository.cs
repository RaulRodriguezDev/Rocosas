using Rocosa.Models;

namespace Rocosa.Data.Repository.IRepository
{
    public interface IUserApplicationRepository : IRepository<UserApplication>
    {
        void Update(UserApplication userApplication);
    }
}
