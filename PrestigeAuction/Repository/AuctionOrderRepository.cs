using PrestigeAuction.Data;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Models;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using PrestigeAuction.Utility;

namespace PrestigeAuction.Repository
{
    public class AuctionOrderRepository : Repository<AuctionOrder>, IAuctionOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public AuctionOrderRepository(ApplicationDbContext db) : base(db)
        {
            _context = db;
        }

        public IOrderedQueryable<AuctionOrder> GetAllOrderedByDate(string userId)
        {
            var myOrders = GetAll(includeProperty: "Product")
                                .Where(u => u.UserId == userId).OrderBy(o => o.OrderDate);
            return myOrders;
        }

        public IOrderedQueryable<AuctionOrder> GetAllOrderedByDeliveryStatus()
        {
            var pendingOrders = GetAll(includeProperty: "Product").OrderByDescending(o => o.DeliveryStatus);
            return pendingOrders;
        }

        public async Task SaveA()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(AuctionOrder auctionOrder)
        {
            _context.AuctionOrders.Update(auctionOrder);
        }

        public void UpdateStatus(Guid orderId, string deliveryStatus, string? paymentStatus = null)
        {
            var auctionOrder = _context.AuctionOrders.FirstOrDefault(o => o.OrderId == orderId);
            if (auctionOrder != null)
            {
                auctionOrder.DeliveryStatus = deliveryStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    auctionOrder.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(Guid orderId, string sessionId, string paymentIntentId)
        {
            var auctionOrder = _context.AuctionOrders.FirstOrDefault(o => o.OrderId == orderId);
            if (auctionOrder != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    auctionOrder.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    auctionOrder.PaymentIntentId = paymentIntentId;
                    auctionOrder.PaymentDate = DateTime.Now.ToLocalTime();
                }
            }
        }
    }
}
