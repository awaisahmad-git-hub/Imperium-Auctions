using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ImperiumAuctions.Models;

namespace ImperiumAuctions.ViewModel
{
    public class ProductViewModel
    {
        public Product? Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
       /* public IEnumerable<Product> Products { get; set; }*/
 /*       public string? SearchString { get; set; }*/

    }
}
