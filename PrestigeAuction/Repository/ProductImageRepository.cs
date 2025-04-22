using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Models;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PrestigeAuction.Repository
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductImageRepository(ApplicationDbContext db):base(db)
        {
            _context = db;
        }

        public void Update(ProductImage productImage)
        {
            _context.ProductImages.Update(productImage);
        }
        /*public IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter)
        {
            IQueryable<ProductImage> query = _context.ProductImages.Where(filter);
            return query;
        }*/
    }
}
