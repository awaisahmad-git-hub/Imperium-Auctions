using PrestigeAuction.Models;
using System.Linq.Expressions;

namespace PrestigeAuction.Repository.IRepository
{
    public interface IBidRepository:IRepository<Bid>
    {
        void Update(Bid bid);
        Task SaveA();
        Task<double> MaxBid(int? id);

        //IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter);
    }
}
