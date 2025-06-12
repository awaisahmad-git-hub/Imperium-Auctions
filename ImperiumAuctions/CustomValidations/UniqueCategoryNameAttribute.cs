using ImperiumAuctions.Data;
using ImperiumAuctions.Models;
using ImperiumAuctions.Repository.IRepository;
using System.ComponentModel.DataAnnotations;

namespace ImperiumAuctions.CustomValidations
{
    public class UniqueCategoryNameAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = value?.ToString()?.ToLower().Trim();
            var _context = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            if (_context!=null&&_context.Categories.Any(c => c.Name != null && c.Name.ToLower() == currentValue))
            {
                return new ValidationResult("Category name already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
