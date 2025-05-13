using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrestigeAuction.DTOs;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Utility;
using Stripe.Checkout;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PrestigeAuction.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class AuctionOrderController : Controller
    {
        private readonly IMainRepository _MainRepo;
        private readonly UserManager<IdentityUser> _UserManager;
        public AuctionOrderController(IMainRepository mainRepo, UserManager<IdentityUser> userManager)
        {
            _MainRepo = mainRepo;
            _UserManager = userManager;
        }
        [HttpGet("user/auction-order/order-summary/{bidId}")]
        public async Task<IActionResult> OrderSummary(Guid bidId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bid = _MainRepo.BidRepository.Get(b => b.BidID == bidId && b.UserId == userId, includeProperty: "Product");
            if (bid != null)
            {
                var maxBid = await _MainRepo.BidRepository.MaxBid(bid.ProductID);
                if (maxBid == bid.BidPrice)
                {
                    var user = await _UserManager.GetUserAsync(User) as ApplicationUser;
                    var countDown = _MainRepo.CountDownTargetRepository.Get(e => e.ProductID == bid.ProductID);
                    AuctionOrder auctionOrder = new()
                    {
                        Name = user?.Name,
                        PhoneNumber = user?.PhoneNumber,
                        ShippingAddress = user?.Address,
                        PostalCode = user?.PostalCode,
                        FinalBidPrice = bid.BidPrice,
                        ProductID = bid.ProductID,
                        PaymentDueDate = (countDown ?? new()).EndTargetDate.AddDays(2),
                        Product = bid.Product,
                    };
                    return View(auctionOrder);
                }
            }
            return View();
        }

        [HttpPost("user/auction-order/order-summary/{bidId}")]
        public async Task<IActionResult> OrderSummary(AuctionOrder auctionOrder)
        {
            if (auctionOrder == null) { return NotFound(); }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bid = _MainRepo.BidRepository.Get(b => b.ProductID == auctionOrder.ProductID && b.UserId == userId, includeProperty: "Product");
            if (bid != null)
            {
                var maxBid = await _MainRepo.BidRepository.MaxBid(bid.ProductID);
                if (maxBid == bid.BidPrice)
                {
                    var countDown = _MainRepo.CountDownTargetRepository.Get(e => e.ProductID == bid.ProductID);
                    var auction_order = _MainRepo.AuctionOrderRepository.Get(u => u.UserId == userId && u.ProductID == bid.ProductID);
                    if (auction_order != null)
                    {
                        auction_order.Name = auctionOrder.Name;
                        auction_order.PhoneNumber = auctionOrder.PhoneNumber;
                        auction_order.ShippingAddress = auctionOrder.ShippingAddress;
                        auction_order.PostalCode = auctionOrder.PostalCode;
                    }
                    else
                    {
                        auctionOrder.OrderId = Guid.NewGuid();
                        auctionOrder.OrderDate = (countDown ?? new()).EndTargetDate;
                        auctionOrder.PaymentDueDate = (countDown ?? new()).EndTargetDate.AddDays(2);
                        auctionOrder.FinalBidPrice = bid.BidPrice;
                        auctionOrder.ProductID = bid.ProductID;
                        auctionOrder.UserId = bid.UserId;
                        auctionOrder.DeliveryStatus = StaticValues.DeliveryStatusPending;
                        auctionOrder.PaymentStatus = StaticValues.PaymentStatusPending;
                        _MainRepo.AuctionOrderRepository.Add(auctionOrder);
                    }
                    await _MainRepo.AuctionOrderRepository.SaveA();
                }
            }
            var newAuctionOrder = _MainRepo.AuctionOrderRepository
                .Get(u => u.UserId == userId && u.ProductID == auctionOrder.ProductID, includeProperty: "Product");
            if (newAuctionOrder != null)
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = $"https://localhost:7002/user/auction-order/order-confirmation/{newAuctionOrder.OrderId}",
                    CancelUrl = $"https://localhost:7002/user/auction-order/order-summary/{bid?.BidID}",
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData=new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(newAuctionOrder.FinalBidPrice * 100), // Stripe needs amount in paisa
                                Currency = "pkr",
                                ProductData=new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name=newAuctionOrder.Product?.Title
                                }
                            },
                            Quantity=1
                        },
                    },
                    Mode = "payment",
                };
                var service = new SessionService();
                Session session = service.Create(options);
                _MainRepo.AuctionOrderRepository.UpdateStripePaymentId(newAuctionOrder.OrderId, session.Id, session.PaymentIntentId);
                await _MainRepo.AuctionOrderRepository.SaveA();

                Response.Headers.Append("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(OrderConfirmation), new { orderId = newAuctionOrder?.OrderId });
        }
        [HttpGet("user/auction-order/order-confirmation/{orderId}")]
        public async Task<IActionResult> OrderConfirmation(Guid orderId)
        {
            var auctionOrder = _MainRepo.AuctionOrderRepository.Get(u => u.OrderId == orderId);
            if (auctionOrder == null) { return NotFound(); }
            var service = new SessionService();
            Session session = service.Get(auctionOrder.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _MainRepo.AuctionOrderRepository.UpdateStripePaymentId(orderId, session.Id, session.PaymentIntentId);
                _MainRepo.AuctionOrderRepository.UpdateStatus(orderId, StaticValues.DeliveryStatusPending, StaticValues.PaymentStatusApproved);
                auctionOrder.PaymentDate = DateTime.UtcNow.ToLocalTime();
                await _MainRepo.AuctionOrderRepository.SaveA();
            }

            return View();
        }
        [HttpGet("user/auction-order/my-orders")]
        public IActionResult MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) { return NotFound(); }
            var auctionOrders = _MainRepo.AuctionOrderRepository.GetAllOrderedByDate(userId);
            return View(auctionOrders);
        }

        #region Api methods
        [HttpPost]
        public async Task<IActionResult> SaveChanges([FromBody] AuctionOrderDTO auctionOrderDTO)
        {
            var user = await _UserManager.GetUserAsync(User) as ApplicationUser;
            if (user != null)
            {
                user.Name = auctionOrderDTO.sdName;
                user.PhoneNumber = auctionOrderDTO.sdPhoneNumber;
                user.Address = auctionOrderDTO.sdShippingAddress;
                user.PostalCode = auctionOrderDTO.sdPostalCode;
                var result = await _UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Changed Successfully" });
                }
                else
                {
                    // handle errors
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Ok();
        }
        #endregion
    }
}
