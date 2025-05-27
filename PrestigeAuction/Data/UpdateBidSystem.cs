using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PrestigeAuction.DTOs;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.ViewModel;

namespace PrestigeAuction.Data
{
    public class UpdateBidSystem : Hub
    {
        private readonly IMainRepository _MainRepo;
        private readonly UserManager<IdentityUser> _UserManager;
        private static Dictionary<string, string> connectionToProduct = new();
        public UpdateBidSystem(IMainRepository mainRepo, UserManager<IdentityUser> userManager)
        {
            _MainRepo = mainRepo;
            _UserManager = userManager;
        }
        public async Task JoinGroup(string productId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, productId);
            connectionToProduct[Context.ConnectionId] = productId;
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (connectionToProduct.TryGetValue(Context.ConnectionId, out var productId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, productId);
                connectionToProduct.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task Received(string message, int productId)
        {
            var userId = Context.UserIdentifier;
            if (userId == null) return;
            var bid = _MainRepo.BidRepository.Get(u => u.ProductID == productId && u.UserId == userId);
            if (bid == null) return;
            var countDown = _MainRepo.CountDownTargetRepository.Get(u => u.ProductID == bid.ProductID);
            if (countDown?.EndTargetDate <= DateTime.UtcNow.ToLocalTime()) return;
            if (Context.User == null) return;
            var user = await _UserManager.GetUserAsync(Context.User) as ApplicationUser;
            var userName = user?.Name;
            if (userName == null) return;

            ChatMessage chatMessage = new()
            {
                MessageID = Guid.NewGuid(),
                Message = message,
                ProductID = productId,
                SenderId = userId,
                SenderName = userName,
            };
            _MainRepo.ChatMessageRepository.Add(chatMessage);
            await _MainRepo.SaveA();
            await Clients.Group(productId.ToString()).SendAsync("Received", message,userName,userId,chatMessage.Timestamp);

        }
    }
}

