using System.Linq.Expressions;

namespace Rocosa.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll(Expression<Func<T,bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null, bool isTracking = true);
        T GetFirst(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool isTracking = true);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Record();
    }
}
