using PrestigeAuction.Models;
using System.Linq.Expressions;

namespace PrestigeAuction.Repository.IRepository
{
    public interface ICountDownTargetRepository : IRepository<CountDownTarget>
    {
        void Update(CountDownTarget countDownTarget);
        Task SaveA();

        //IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter);
    }
}
