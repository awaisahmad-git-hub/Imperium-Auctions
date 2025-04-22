using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Models;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;

namespace PrestigeAuction.Repository
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        private readonly ApplicationDbContext _context;
        public BidRepository(ApplicationDbContext db):base(db)
        {
            _context = db;
        }

        public async Task<double> MaxBid(int? id)
        {
            var maxBid = await _context.Bids.Where(u=>u.ProductID==id).Select(u => u.BidPrice).DefaultIfEmpty().MaxAsync();
            return maxBid;
        }

        public async Task SaveA()
        {
           await _context.SaveChangesAsync();
        }

        public void Update(Bid bid)
        {
            _context.Bids.Update(bid);
        }
        /*public IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter)
        {
            IQueryable<ProductImage> query = _context.ProductImages.Where(filter);
            return query;
        }*/
    }
}
