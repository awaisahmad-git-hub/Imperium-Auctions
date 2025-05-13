using PrestigeAuction.Models;
using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrestigeAuction.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public IOrderedQueryable<Product> GetAllOrderedByTitle(string? includeProperty = null)
        {
            return GetAll(includeProperty).OrderBy(o=>o.Title);
        }

        public void Update(Product product)
        {
            var obj = _context.Products.FirstOrDefault(u => u.Id == product.Id);
            if (obj != null)
            {
                obj.Title = product.Title;
                obj.Description = product.Description;
                obj.SKU = product.SKU;
                obj.StartingPrice = product.StartingPrice;
                obj.CategoryId = product.CategoryId; 
                obj.ProductImageList = product.ProductImageList;    
            }
        }
    }
}
