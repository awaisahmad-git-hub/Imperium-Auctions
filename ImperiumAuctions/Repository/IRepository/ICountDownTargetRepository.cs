using ImperiumAuctions.Models;
using System.Linq.Expressions;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface ICountDownTargetRepository : IRepository<CountDownTarget>
    {
        void Update(CountDownTarget countDownTarget);
        Task SaveA();

        //IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter);
    }
}
