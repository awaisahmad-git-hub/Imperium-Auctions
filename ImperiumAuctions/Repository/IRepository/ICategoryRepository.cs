using ImperiumAuctions.Models;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
        IOrderedQueryable<Category> GetAllOrderedByName(string? includeProperty = null);
    }
}
