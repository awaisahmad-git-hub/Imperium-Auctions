using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using PrestigeAuction.Data;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.ViewModel;
using System.Security.Claims;
using System.Security.Cryptography;

namespace PrestigeAuction.Areas.User.Controllers
{
    [Area("User")]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
 
            var _bid = _MainRepo.BidRepository.Get(u => u.ProductID == productId && u.UserId == userId);
            if (_bid != null)
            {
                _MainRepo.BidRepository.Delete(_bid);
            }
            
            Bid bid = new() 
            {
                BidID=Guid.NewGuid(),
                BidPrice= bidValue,
                BidTime=DateTime.UtcNow,
                ProductID= productId,
                UserId=userId
            };
            _MainRepo.BidRepository.Add(bid);
            await _MainRepo.BidRepository.SaveA();
            await _hubContext.Clients.All.SendAsync("PlaceBid",bid);
            return Ok(new {message= "Placed Successfully" });
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
    }
}
