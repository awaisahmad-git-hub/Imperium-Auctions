using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrestigeAuction.Models;

namespace PrestigeAuction.ViewModel
{
    public class Product_Bid_MaxBid_CountDownTargetViewModel
    {
        public Product? Product { get; set; }
        public CountDownTarget? CountDownTarget { get; set; }
        public Bid? Bid { get; set; }
        public double MaxBid { get; set; }
    }
}
