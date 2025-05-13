using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeAuction.Models
{
    public class AuctionOrder
    {
        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDueDate { get; set; }

        [Required]
        public double FinalBidPrice { get; set; }

        public string? PaymentStatus { get; set; }
        public string? DeliveryStatus { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required]
        [DisplayName("Shipping Address")]
        public string? ShippingAddress { get; set; }
        [Required]
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        public int? ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product? Product { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser? User { get; set; }
    }
}
