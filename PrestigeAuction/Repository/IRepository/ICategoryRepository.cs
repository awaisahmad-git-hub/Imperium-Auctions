using PrestigeAuction.Models;

namespace PrestigeAuction.Repository.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
    }
}
