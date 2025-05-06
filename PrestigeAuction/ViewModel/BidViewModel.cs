using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrestigeAuction.Models;

namespace PrestigeAuction.ViewModel
{
    public class BidViewModel
    {
        public Product? Product { get; set; }
        public CountDownTarget? CountDownTarget { get; set; }
        public Bid? Bid { get; set; }
        public double MaxBid { get; set; }
        public double CurrentUserMaxBid { get; set; }
    }
}
