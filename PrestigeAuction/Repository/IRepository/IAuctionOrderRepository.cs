using PrestigeAuction.Models;
using Stripe.Climate;
using System.Linq.Expressions;

namespace PrestigeAuction.Repository.IRepository
{
    public interface IAuctionOrderRepository : IRepository<AuctionOrder>
    {
        Task SaveA();
        void Update(AuctionOrder auctionOrder);
        /// <summary>
        /// Updates the delivery and payment status of a specific order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="deliveryStatus">The new delivery status to set.</param>
        /// <param name="paymentStatus">
        /// The new payment status to set (optional). 
        /// If null, the payment status will not be changed.
        /// </param>
        void UpdateStatus(Guid orderId, string deliveryStatus, string? paymentStatus = null);
        /// <summary>
        /// Updates the Stripe session ID and payment intent ID for a specific order.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to update.</param>
        /// <param name="sessionId">The Stripe session ID associated with the order.</param>
        /// <param name="paymentIntentId">The Stripe payment intent ID for the transaction.</param>
        void UpdateStripePaymentId(Guid orderId, string sessionId, string paymentIntentId);
        /// <summary>
        /// Retrieves all orders of the specified user, ordered by date.
        /// </summary>
        /// <param name="userId">The ID of the user whose orders are to be retrieved.</param>
        /// <returns>
        /// An <see cref="IOrderedQueryable{AuctionOrder}"/> containing the user's orders,
        /// or an empty sequence if none are found.
        /// </returns>
        IOrderedQueryable<AuctionOrder> GetAllOrderedByDate(string userId);
        /// <summary>
        /// Retrieves all orders, ordered by order date.
        /// </summary>
        /// <returns>
        /// An <see cref="IOrderedQueryable{AuctionOrder}"/> containing the orders,
        /// or an empty sequence if none are found.
        /// </returns>
        IOrderedQueryable<AuctionOrder> GetAllOrderedByDeliveryStatus();
    }
}
