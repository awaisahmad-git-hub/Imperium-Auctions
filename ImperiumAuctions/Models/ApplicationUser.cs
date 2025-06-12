using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ImperiumAuctions.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(40)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string? Address { get; set; }


        [Display(Name = "Postal Code")]
        [StringLength (20)]
        public string? PostalCode { get; set; }
    }
}
