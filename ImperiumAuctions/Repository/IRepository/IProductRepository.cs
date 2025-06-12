using ImperiumAuctions.Models;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
        IOrderedQueryable<Product> GetAllOrderedByTitle(string? includeProperty = null);
    }
}
