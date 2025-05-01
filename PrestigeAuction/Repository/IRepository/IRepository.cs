using System.Linq.Expressions;

namespace PrestigeAuction.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(string? includeProperty = null);
        T? Get(Expression<Func<T,bool>> filter,string? includeProperty = null);
        void Add(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
