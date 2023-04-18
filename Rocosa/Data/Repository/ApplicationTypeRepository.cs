using Rocosa.Data.Repository.IRepository;
using Rocosa.Models;

namespace Rocosa.Data.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationTypeRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public void Update(ApplicationType applicationType)
        {
            var previousType = _context.ApplicationType.FirstOrDefault(c => c.Id == applicationType.Id);

            if (previousType != null)
            {
                previousType.Name= applicationType.Name;
            }
        }
    }
}
