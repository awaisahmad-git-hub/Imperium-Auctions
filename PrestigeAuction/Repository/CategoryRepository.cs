using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Models;

namespace PrestigeAuction.Repository
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
    }
}
