using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class UserApplicationRepository : Repository<UserApplication>, IUserApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public UserApplicationRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }
        public void Update(UserApplication userApplication)
        {
            _context.Update(userApplication);
        }

    }
}
