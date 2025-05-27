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
using System;
using System.Globalization;
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
        #region Api methods
        [HttpPost]
        public async Task<IActionResult> PlaceBid([FromBody] PlaceBidDTO placeBidDTO)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }
            BidViewModel bidViewModel = new()
            {
                MaxBid = await _MainRepo.BidRepository.MaxBid(placeBidDTO.ProductId)
            };
            if (placeBidDTO.BidValue <= bidViewModel.MaxBid)
            {
                return BadRequest(new {message= "The Bid Price must be greater than current bid." });
            }
            if ((placeBidDTO.BidValue - bidViewModel.MaxBid) < 500)
            {
                return BadRequest(new { message = "The minimum Bid Price must be greater than 500 from current bid." });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bid= _MainRepo.BidRepository.Get(u => u.ProductID == placeBidDTO.ProductId && u.UserId == userId);
            if (bid != null)
            {
                bid.BidPrice = placeBidDTO.BidValue;
                bid.BidTime = DateTime.UtcNow.ToLocalTime();
                await _hubContext.Clients.All.SendAsync("PlaceBid", new
                {
                    bidPrice = bid.BidPrice,
                    userId = bid.UserId
                });
            }
            else
            {
                Bid newBid = new()
                {
                    BidID=Guid.NewGuid(),
                    BidPrice = placeBidDTO.BidValue,
                    BidTime = DateTime.UtcNow.ToLocalTime(),
                    ProductID = placeBidDTO.ProductId,
                    UserId = userId
                };
                _MainRepo.BidRepository.Add(newBid);
                await _hubContext.Clients.All.SendAsync("PlaceBid", new
                {
                    bidPrice = newBid.BidPrice,
                    userId = newBid.UserId,
                });
            }
            await _MainRepo.BidRepository.SaveA();
            return Ok(new {message= "Placed Successfully"});
        }
        [HttpPost]
        public async Task<IActionResult> CountDownTargetTime([FromBody] CountDownDTO countDownDTO)
        {
            var countDownTarget=_MainRepo.CountDownTargetRepository.Get(u=>u.ProductID == countDownDTO.ProductId);
            if (countDownTarget != null) {
                countDownTarget.StartTargetDate = countDownDTO.StartTargetDate.ToLocalTime();
                countDownTarget.EndTargetDate = countDownDTO.EndTargetDate.ToLocalTime();
                await _hubContext.Clients.All.SendAsync("CountDown", new
                {
                    startTargetDate=countDownTarget.StartTargetDate,
                    endTargetDate=countDownTarget.EndTargetDate
                });
            }
            else
            {
                CountDownTarget newCountDownTarget = new()
                {
                    StartTargetDate = countDownDTO.StartTargetDate.ToLocalTime(),
                    EndTargetDate = countDownDTO.EndTargetDate.ToLocalTime(),
                    ProductID = countDownDTO.ProductId
                };
                _MainRepo.CountDownTargetRepository.Add(newCountDownTarget);
                await _hubContext.Clients.All.SendAsync("CountDown", new
                {
                    startTargetDate = newCountDownTarget.StartTargetDate,
                    endTargetDate = newCountDownTarget.EndTargetDate
                });
            }
            await _MainRepo.CountDownTargetRepository.SaveA();
            return Ok();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuctionEndNotification([FromBody] BidProductIdDTO bidProductIdDTO)
        {
            if (bidProductIdDTO != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                BidViewModel bidViewModel = new()
                {
                    Bid = _MainRepo.BidRepository.Get(u => u.ProductID == bidProductIdDTO.ProductId && u.UserId == userId),
                    CountDownTarget = _MainRepo.CountDownTargetRepository.Get(u => u.ProductID == bidProductIdDTO.ProductId),
                    MaxBid = await _MainRepo.BidRepository.MaxBid(bidProductIdDTO.ProductId),
                    CurrentUserMaxBid = await _MainRepo.BidRepository.CurrentUserMaxBid(bidProductIdDTO.ProductId, userId)
                };

                if ( bidViewModel.Bid != null&& bidViewModel.CountDownTarget?.EndTargetDate <= DateTime.UtcNow.ToLocalTime())
                {
                    bidViewModel.Bid.IsBidEndNotificationSeen = true;
                    await _MainRepo.BidRepository.SaveA();
                    if (bidViewModel.CurrentUserMaxBid == bidViewModel.MaxBid)
                    {
                        var winnerNotification = $"Congratulations! 🎉 You have won the auction! Your winning bid is {bidViewModel.MaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))}. Please complete your payment within 48 hours. Otherwise, you will lose the product.";
                        return Ok(new { message = winnerNotification,isWinner=true,bidId=bidViewModel.Bid.BidID });
                    }
                    else
                    {
                        var loserNotification = $"Unfortunately, you didn’t win this auction. Your bid was {bidViewModel.CurrentUserMaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))} while the winning bid was {bidViewModel.MaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))}. Don’t worry! — more auctions are waiting for you.";
                        return Ok(new { message = loserNotification , isWinner = false });
                    }
                }
            }
            Console.WriteLine("Your message here");
            return NoContent();
        }
        #endregion
    }
}
