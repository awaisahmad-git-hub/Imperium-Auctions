using Microsoft.EntityFrameworkCore;
using PrestigeAuction.Data;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using System.Linq.Expressions;

namespace PrestigeAuction.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _context = db;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);     //  _context.Categories = _dbSet
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperty = null)
        {
            IQueryable<T> query = _dbSet.Where(filter);     //  _context.Categories.Where(u=>u.Id==id)
            
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var property in includeProperty.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();                  //  _context.Categories.Where(u=>u.Id==id).FirstOrDefault()
        }

        public IQueryable<T> GetAll(string? includeProperty=null)
        {
            IQueryable<T> query = _dbSet;
            if (includeProperty != null)
            {
                foreach (var property in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            
            return query;
        }
    }
}
