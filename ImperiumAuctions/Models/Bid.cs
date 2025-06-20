﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using ImperiumAuctions.Data;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.ViewModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperiumAuctions.Models
{
    public class Bid
    {
        [Key]
        public Guid BidID { get; set; }
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The Price must be above 0")]
        [DisplayName("Bid Price:")]
        
        public double BidPrice { get; set; }
        [Required]
        public DateTime BidTime { get; set; }
        [Required]
        public int ProductID { get; set; } 
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product? Product { get; set; }
        [Required]
        [StringLength(450)]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public IdentityUser? User { get; set; }
        public bool IsBidEndNotificationSeen { get; set; }

    }
}

