using Microsoft.AspNetCore.SignalR;

namespace PrestigeAuction.Data
{
    public class UpdateBidSystem:Hub
    {
        public async Task Received(string message,int productId)
        {
            
            await Clients.All.SendAsync("Received", message, productId);
        }
    }
}

