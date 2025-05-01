using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.ViewModel;
using System.Diagnostics;

namespace PrestigeAuction.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IMainRepository _MainRepo;
        public HomeController(IMainRepository mainRepo)
        {
            _MainRepo = mainRepo;
        }
        [Route("/")]
        public IActionResult Index(string? searchString)
        {
            ProductViewModel productViewModel = new ProductViewModel()
            {
                CategoryList = _MainRepo.CategoryRepository.GetAllOrderedByName().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList()
            };
            ViewBag.CategoryModel = productViewModel;
            if (!string.IsNullOrEmpty(searchString))
            {
                var products = _MainRepo.ProductRepository
               .GetAll(includeProperty: "ProductImageList,CountDownTarget")
               .Where(p => p.Title != null && p.Title.ToUpper().Contains(searchString.ToUpper()))
               .OrderBy(o => o.Title);
                return View(products);
            }
            else
            {
                var products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList,CountDownTarget");
                return View(products);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> ProductDetails(int? id)
        {
            Product_Bid_MaxBid_CountDownTargetViewModel obj = new()
            {
                Product = _MainRepo.ProductRepository.Get(u => u.Id == id, includeProperty: "Category,ProductImageList"),
                Bid = new Bid(),
                CountDownTarget = _MainRepo.CountDownTargetRepository.Get(u=>u.ProductID==id),
                MaxBid = await _MainRepo.BidRepository.MaxBid(id)
            };
            return View(obj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region Api methods
        public IActionResult SearchHomeProductByCategory(string? searchString)
        {
            IQueryable<Product> Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (searchString == "All")
                {
                    Products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList");
                }
                else
                {
                    Products = _MainRepo.ProductRepository
                   .GetAll(includeProperty: "ProductImageList")
                   .Where(p => p.Category != null && p.Category.Name != null && p.Category.Name.ToUpper().Contains(searchString.ToUpper()))
                   .OrderBy(o => o.Title);
                }
            }
            else
            {
                Products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList");
            }
            return PartialView("_HomeProducts", Products.ToList());
        }
        public IActionResult SearchHomeProductByName(string? searchString)
        {
            IOrderedQueryable<Product> products;
            if (!string.IsNullOrEmpty(searchString))
            {                    
                products = _MainRepo.ProductRepository
                   .GetAll(includeProperty: "ProductImageList")
                   .Where(p => p.Title != null && p.Title.ToUpper().Contains(searchString.ToUpper()))
                   .OrderBy(o => o.Title);
                return PartialView("_HomeProducts", products);
            }
            products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList");
            return PartialView("_HomeProducts", products);
        }
        #endregion
    }

}
