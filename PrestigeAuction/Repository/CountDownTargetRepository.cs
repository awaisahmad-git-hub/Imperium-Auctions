using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PrestigeAuction.Repository
{
    public class CountDownTargetRepository : Repository<CountDownTarget>, ICountDownTargetRepository
    {
        private readonly ApplicationDbContext _context;
        public CountDownTargetRepository(ApplicationDbContext db):base(db)
        {
            _context = db;
        }
        /*public async Task<CountDownTarget?> GetTargetDate(int? id)
        {
            var target = await _context.CountDownTargets
            return target;
        }*/
        public void Update(CountDownTarget countDownTarget)
        {
            _context.CountDownTargets.Update(countDownTarget);
        }
        public async Task SaveA()
        {
           await _context.SaveChangesAsync();
        }

        
        /*public IQueryable<ProductImage> GetAllImages(Expression<Func<ProductImage, bool>> filter)
        {
            IQueryable<ProductImage> query = _context.ProductImages.Where(filter);
            return query;
        }*/
    }
}
