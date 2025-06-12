using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImperiumAuctions.Models;
using ImperiumAuctions.Repository;

namespace ImperiumAuctions.ViewModel
{
    public class BidViewModel
    {
        public Product? Product { get; set; }
        public CountDownTarget? CountDownTarget { get; set; }
        public Bid? Bid { get; set; }
        public double MaxBid { get; set; }
        public double CurrentUserMaxBid { get; set; }
        public IOrderedQueryable<ChatMessage>? ChatMessages { get; set; }
        public string? CurrentUserId { get; set; }
    }
}
