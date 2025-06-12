using ImperiumAuctions.Data;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Models;
using Microsoft.EntityFrameworkCore;

namespace ImperiumAuctions.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _context = db;
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
        public IOrderedQueryable<Category> GetAllOrderedByName(string? includeProperty = null)
        {
            return GetAll(includeProperty).OrderBy(o=>o.Name);
        }
    }
}
