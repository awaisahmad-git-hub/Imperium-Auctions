using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperiumAuctions.Models
{
    public class CountDownTarget
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime StartTargetDate { get; set; }
        [Required]
        public DateTime EndTargetDate { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey(nameof(ProductID))]
        [ValidateNever]
        public Product? Product { get; set; }
    }
}

