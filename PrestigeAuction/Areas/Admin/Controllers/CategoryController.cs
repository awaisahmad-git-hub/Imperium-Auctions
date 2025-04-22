using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrestigeAuction.Data;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.Utility;

namespace PrestigeAuction.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticValues.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IMainRepository _MainRepo;
        public CategoryController(IMainRepository MainRepo)
        {
            _MainRepo = MainRepo;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("admin/product-categories")]
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _MainRepo.CategoryRepository.GetAll();
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
            Category? category = _MainRepo.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
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
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _MainRepo.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return RedirectToAction("Index", "Category");
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? category = _MainRepo.CategoryRepository.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _MainRepo.CategoryRepository.Delete(category);
            _MainRepo.Save();
            TempData["success"] = "Deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
