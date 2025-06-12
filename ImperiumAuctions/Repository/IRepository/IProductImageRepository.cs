using ImperiumAuctions.Models;
using System.Linq.Expressions;

namespace ImperiumAuctions.Repository.IRepository
{
    public interface IProductImageRepository:IRepository<ProductImage>
    {
        void Update(ProductImage productImage);
        //IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter);
    }
}
