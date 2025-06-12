using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using ImperiumAuctions.Data;
using ImperiumAuctions.Models;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Utility;
using ImperiumAuctions.ViewModel;
using System.Collections.Generic;

namespace ImperiumAuctions.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = StaticValues.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IMainRepository _MainRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IMainRepository MainRepo, IWebHostEnvironment webHostEnvironment)
        {
            _MainRepo = MainRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [HttpGet("admin/products")]
        public IActionResult Index(string? searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var products = _MainRepo.ProductRepository
               .GetAll(includeProperty: "Category")
               .Where(p => p.Category != null && p.Category.Name != null && p.Category.Name.ToUpper().Contains(searchString.ToUpper()))
               .OrderBy(o => o.Title);
                return View(products);
            }
            else
            {
                var products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "Category");
                return View(products);
            }
        }

        [HttpGet("admin/product/create")]
        [HttpGet("admin/product/update/{id}")]
        public IActionResult CreateUpdate(int? id)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoryList = _MainRepo.CategoryRepository.GetAllOrderedByName().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList()

            };
            if (id == null || id == 0)
            {

                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _MainRepo.ProductRepository.Get(u => u.Id == id, includeProperty: "ProductImageList,Category");

                if (productViewModel.Product == null)
                {
                    return RedirectToAction("Index", "Product");
                }
                return View(productViewModel);
            }
        }

        [HttpPost("admin/product/create")]
        [HttpPost("admin/product/update/{id}")]
        public IActionResult CreateUpdate(ProductViewModel PVM, List<IFormFile> files)
        {
            if (files != null)
            {
                var invalidFiles = new List<string>();
                foreach (IFormFile file in files)
                {
                    var validImageTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (!validImageTypes.Contains(fileExtension))
                    {
                        invalidFiles.Add(file.FileName);
                    }
                }
                if (invalidFiles.Any())
                {
                    ModelState.AddModelError("file", $"The following files are invalid: {string.Join(", ", invalidFiles)}. Use these types of files (jpg, jpeg, png, gif).");
                    if (PVM.Product?.Id != null)
                    {
                        PVM.Product = _MainRepo.ProductRepository.Get(u => u.Id == PVM.Product.Id, includeProperty: "ProductImageList");
                    }
                    PVM.CategoryList = _MainRepo.CategoryRepository.GetAllOrderedByName().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).ToList();
                    return View(PVM);
                }
            }
            if (ModelState.IsValid)
            {
                if (PVM.Product?.Id != null)
                {
                    _MainRepo.ProductRepository.Update(PVM.Product);
                    TempData["success"] = "Updated successfully";
                }
                else if (PVM.Product != null)
                {
                    _MainRepo.ProductRepository.Add(PVM.Product);
                    TempData["success"] = "Created successfully";
                }
                _MainRepo.Save();
                if (files != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    foreach (IFormFile file in files)
                    {
                        string productPath = @"image\products\product-" + PVM.Product?.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);
                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ProductImage productImage = new()
                        {
                            ImageURL = @"\" + productPath + @"\" + fileName,
                            ProductID = PVM.Product?.Id ?? 0
                        };
                        if (PVM.Product?.ProductImageList == null && PVM.Product != null)
                        {
                            PVM.Product.ProductImageList = new List<ProductImage>();
                        }
                        PVM.Product?.ProductImageList?.Add(productImage);
                    }
                    if (PVM.Product != null)
                    {
                        _MainRepo.ProductRepository.Update(PVM.Product);
                        _MainRepo.Save();
                    }
                }
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        #region Api methods
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var product = _MainRepo.ProductRepository.Get(c => c.Id == id);
                if (product != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string imageDirectoryPath = Path.Combine(wwwRootPath, @"image\products\product-" + id);
                    if (Directory.Exists(imageDirectoryPath))
                    {
                        Directory.Delete(imageDirectoryPath, true);
                    }
                    _MainRepo.ProductRepository.Delete(product);
                    _MainRepo.Save();
                    var products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "Category");
                    return PartialView("_ProductTableBody", products);
                }
            }
            return NotFound();
        }
        public IActionResult DeleteImage(int? id)
        {
            if (id != null)
            {
                var productImage = _MainRepo.ProductImageRepository.Get(c => c.ImageID == id);
                if (productImage != null)
                {
                    if (!string.IsNullOrEmpty(productImage.ImageURL))
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string ImagePath = Path.Combine(wwwRootPath, productImage.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                        string imageDirectoryPath = Path.Combine(wwwRootPath, @"image\products\product-" + productImage.ProductID);
                        if (Directory.Exists(imageDirectoryPath) && !Directory.EnumerateFileSystemEntries(imageDirectoryPath).Any())
                        {
                            Directory.Delete(imageDirectoryPath);
                        }
                    }
                    _MainRepo.ProductImageRepository.Delete(productImage);
                    _MainRepo.Save();
                    ProductViewModel productVM = new ProductViewModel()
                    {
                        Product = _MainRepo.ProductRepository.Get(c => c.Id == productImage.ProductID, includeProperty: "ProductImageList"),
                    };
                    return PartialView("_ProductImages", productVM);
                }
            }
            return NotFound();
        }
        public IActionResult SearchProduct(string? searchString)
        {
            IOrderedQueryable<Product> products;
            if (!string.IsNullOrEmpty(searchString))
            {
                products = _MainRepo.ProductRepository
               .GetAll(includeProperty: "Category")
               .Where(p => p.Category != null && p.Category.Name != null && p.Category.Name.ToUpper().Contains(searchString.ToUpper()))
               .OrderBy(o => o.Title);
            }
            else
            {
                products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "Category");
            }
            return PartialView("_ProductTableBody", products);
        }
        #endregion
    }
}