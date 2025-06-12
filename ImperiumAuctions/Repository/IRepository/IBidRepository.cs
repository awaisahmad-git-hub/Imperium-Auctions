using ImperiumAuctions.Models;
using System.Linq.Expressions;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface IBidRepository:IRepository<Bid>
    {
        void Update(Bid bid);
        Task SaveA();
        Task<double> MaxBid(int? id);
        Task<double> CurrentUserMaxBid(int? id, string? currentUserId);

        //IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter);
    }
}
