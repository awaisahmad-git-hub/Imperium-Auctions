using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PrestigeAuction.Models
{
    public class Product
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [DisplayName("Product Title")]
        [StringLength(40)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        [StringLength(40)]
        public string? SKU { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The Price must be above 0")]
        [Display(Name = "Starting Price")]
        public double StartingPrice { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category? Category { get; set; }

        [ValidateNever]
        [Display(Name = "Product Images")]
        public List<ProductImage>? ProductImageList { get; set; }=new List<ProductImage>();
        public CountDownTarget? CountDownTarget { get; set; }
    }
}
