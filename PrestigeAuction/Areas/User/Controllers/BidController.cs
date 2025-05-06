using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using PrestigeAuction.Data;
using PrestigeAuction.DTOs;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.ViewModel;
using Stripe;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PrestigeAuction.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class BidController : Controller
    {
        private readonly IMainRepository _MainRepo;
        private readonly IHubContext<UpdateBidSystem> _hubContext;

        public BidController(IMainRepository mainRepository, IHubContext<UpdateBidSystem> hubContext)
        {
            _MainRepo = mainRepository;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> PlaceBid( int productId, double bidValue)
        {
            BidViewModel bidViewModel = new()
            {
                MaxBid = await _MainRepo.BidRepository.MaxBid(productId)
            };
            if (bidValue<= bidViewModel.MaxBid)
            {
                return BadRequest(new {message= "The Bid Price must be greater than current bid." });
            }
            if ((bidValue - bidViewModel.MaxBid) < 500)
            {
                return BadRequest(new { message = "The minimum Bid Price must be greater than 500 from current bid." });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var _bid= _MainRepo.BidRepository.Get(u => u.ProductID == productId && u.UserId == userId);
            if (_bid != null)
            {
                _MainRepo.BidRepository.Delete(_bid);
            }
            
            Bid bid = new() 
            {
                BidID=Guid.NewGuid(),
                BidPrice= bidValue,
                BidTime=DateTime.UtcNow.ToLocalTime(),
                ProductID= productId,
                UserId=userId
            };
            _MainRepo.BidRepository.Add(bid);
            await _MainRepo.BidRepository.SaveA();
            await _hubContext.Clients.All.SendAsync("PlaceBid", new
            {
                bidPrice=bid.BidPrice,
                userId=bid.UserId
            });
            return Ok(new {message= "Placed Successfully"});
        }
        public async Task<IActionResult> CountDownTargetTime(DateTime startTargetDate, DateTime endTargetDate,int productId)
        {
            var _countDownTarget=_MainRepo.CountDownTargetRepository.Get(u=>u.ProductID ==productId);
            if (_countDownTarget != null) { 
                _MainRepo.CountDownTargetRepository.Delete(_countDownTarget);          
            }
            CountDownTarget countDownTarget = new()
            {
                StartTargetDate = startTargetDate.ToLocalTime(),
                EndTargetDate = endTargetDate.ToLocalTime(),
                ProductID = productId
            };
            _MainRepo.CountDownTargetRepository.Add(countDownTarget);
            await _MainRepo.CountDownTargetRepository.SaveA();
            await _hubContext.Clients.All.SendAsync("CountDown", countDownTarget);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Notification([FromBody] BidProductUserDTO? bidProductUserDTO)
        {
            if (bidProductUserDTO != null)
            {
                BidViewModel bidViewModel = new()
                {
                    Bid = _MainRepo.BidRepository.Get(u => u.ProductID == bidProductUserDTO.ProductId && u.UserId == bidProductUserDTO.UserId),
                    CountDownTarget = _MainRepo.CountDownTargetRepository.Get(u => u.ProductID == bidProductUserDTO.ProductId),
                    MaxBid = await _MainRepo.BidRepository.MaxBid(bidProductUserDTO.ProductId),
                    CurrentUserMaxBid = await _MainRepo.BidRepository.CurrentUserMaxBid(bidProductUserDTO.ProductId, bidProductUserDTO.UserId)
                };

                if (bidViewModel.Bid != null&& bidViewModel.CountDownTarget?.EndTargetDate <= DateTime.UtcNow.ToLocalTime())
                {
                    bidViewModel.Bid.IsBidEndNotificationSeen = true;
                    _MainRepo.BidRepository.Update(bidViewModel.Bid);
                    await _MainRepo.BidRepository.SaveA();
                    await _hubContext.Clients.All.SendAsync("AuctionEndNotification", new
                    {
                        maxBid= bidViewModel.MaxBid,
                        currentUserMaxBid = bidViewModel.CurrentUserMaxBid
                    });
                }
            }
            return Ok();
        }
    }
}
