using PrestigeAuction.CustomValidations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrestigeAuction.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name:")]
        [StringLength(40)]
        [UniqueCategoryName]
        public string? Name { get; set; }
        [DisplayName("Display Order:")]
        [Range(1, 100, ErrorMessage = "Value must be between 1-100")]
        public int DisplayOrder { get; set; }
        public List<Product> ProductList { get; set; } = new List<Product>();
    }
}
