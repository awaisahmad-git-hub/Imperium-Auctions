using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestigeAuction.Models
{
    public class ProductImage
    {
        [Key]
        public int? ImageID { get; set; }
        [Required]
        [StringLength(500)]
        public string? ImageURL { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        [ValidateNever]
        public Product? Product { get; set; }

    }
}
