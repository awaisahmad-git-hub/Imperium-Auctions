using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ImperiumAuctions.Data;
using ImperiumAuctions.Models;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Utility;

namespace ImperiumAuctions.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticValues.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IMainRepository _MainRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CategoryController(IMainRepository MainRepo, IWebHostEnvironment webHostEnvironment)
        {
            _MainRepo = MainRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("admin/product-categories")]
        public IActionResult Index(string? searchString)
        {
            IOrderedQueryable<Category> categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                categories = _MainRepo.CategoryRepository
                 .GetAll().Where(p => p.Name != null && p.Name.ToUpper().Contains(searchString.ToUpper()))
                 .OrderBy(o=>o.Name);
                return View(categories);
            }
            categories = _MainRepo.CategoryRepository.GetAllOrderedByName();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "Both fields don't be same.");
            }
            if (ModelState.IsValid)
            {
                _MainRepo.CategoryRepository.Add(category);
                _MainRepo.Save();
                TempData["success"] = "Created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _MainRepo.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (string.Equals(category.Name,category.DisplayOrder.ToString(),StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("Name", "Both fields don't be same.");
            }
            if (ModelState.IsValid)
            {
                _MainRepo.CategoryRepository.Update(category);
                _MainRepo.Save();
                TempData["success"] = "Updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        #region Api methods
        public IActionResult DeleteCategory(int? id)
        {
            if (id is null) return BadRequest();
            var category = _MainRepo.CategoryRepository.Get(c => c.Id == id, includeProperty: "ProductList");
            if (category is null) return NotFound();
            if (category.ProductList.Any())
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                foreach (var product in category.ProductList)
                {
                    string imageDirectoryPath = Path.Combine(wwwRootPath, @"image\products\product-" + product.Id);
                    if (Directory.Exists(imageDirectoryPath))
                    {
                        Directory.Delete(imageDirectoryPath, true);
                    }
                }
            }
            _MainRepo.CategoryRepository.Delete(category);
            _MainRepo.Save();
            var categories = _MainRepo.CategoryRepository.GetAllOrderedByName();
            return PartialView("_CategoryTableBody", categories.ToList());
        }
        public IActionResult SearchCategory(string? searchString)
        {
            IOrderedQueryable<Category> categories;
            if (!string.IsNullOrEmpty(searchString))
            {
                categories = _MainRepo.CategoryRepository
               .GetAll()
               .Where(p => p.Name != null && p.Name.ToUpper().Contains(searchString.ToUpper())).OrderBy(o => o.Name);
                return PartialView("_CategoryTableBody", categories);
            }
            categories = _MainRepo.CategoryRepository.GetAllOrderedByName();
            return PartialView("_CategoryTableBody", categories);
        }
        #endregion
    }
}
