using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrestigeAuction.Models;
using PrestigeAuction.Repository.IRepository;
using PrestigeAuction.ViewModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;

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
        public async Task<IActionResult> ProductBid(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            BidViewModel bidViewModel = new()
            {
                Product = _MainRepo.ProductRepository.Get(u => u.Id == id, includeProperty: "Category,ProductImageList"),
                Bid = _MainRepo.BidRepository.Get(u => u.ProductID == id && u.UserId == userId),
                CountDownTarget = _MainRepo.CountDownTargetRepository.Get(u => u.ProductID == id),
                MaxBid = await _MainRepo.BidRepository.MaxBid(id),
                CurrentUserMaxBid = await _MainRepo.BidRepository.CurrentUserMaxBid(id, userId),
                ChatMessages = _MainRepo.ChatMessageRepository.GetAllCurrentProductMessages(id),
                CurrentUserId = userId
            };
            // delete bid and chat after the due date
            if (bidViewModel.Bid is not null && bidViewModel.CountDownTarget?.EndTargetDate.AddDays(2) <= DateTime.UtcNow.ToLocalTime())
            {
                _MainRepo.BidRepository.Delete(bidViewModel.Bid);
                _MainRepo.ChatMessageRepository.DeleteRange(bidViewModel.ChatMessages);
                await _MainRepo.SaveA();
            }
            // check if the user is not seen winner or loser notification
            if (bidViewModel.Bid is not null && bidViewModel.Bid.IsBidEndNotificationSeen is false && bidViewModel.CountDownTarget?.EndTargetDate <= DateTime.UtcNow.ToLocalTime())
            {
                bidViewModel.Bid.IsBidEndNotificationSeen = true;
                await _MainRepo.BidRepository.SaveA();
                TempData["WinnerNotification"] = $"Congratulations! 🎉 You have won the auction! Your winning bid is {bidViewModel.MaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))}. Please complete your payment within 48 hours. Otherwise, you will lose the product.";
                TempData["LoserNotification"] = $"Unfortunately, you didn’t win this auction. Your bid was {bidViewModel.CurrentUserMaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))} while the winning bid was {bidViewModel.MaxBid.ToString("'Rs.' #,##0", new CultureInfo("ur-PK"))}. Don’t worry! — more auctions are waiting for you.";
            }
            return View(bidViewModel);
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
                    Products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList,CountDownTarget");
                }
                else
                {
                    Products = _MainRepo.ProductRepository
                   .GetAll(includeProperty: "ProductImageList,CountDownTarget")
                   .Where(p => p.Category != null && p.Category.Name != null && p.Category.Name.ToUpper().Contains(searchString.ToUpper()))
                   .OrderBy(o => o.Title);
                }
            }
            else
            {
                Products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList,CountDownTarget");
            }
            return PartialView("_HomeProducts", Products.ToList());
        }
        public IActionResult SearchHomeProductByName(string? searchString)
        {
            IOrderedQueryable<Product> products;
            if (!string.IsNullOrEmpty(searchString))
            {
                products = _MainRepo.ProductRepository
                   .GetAll(includeProperty: "ProductImageList,CountDownTarget")
                   .Where(p => p.Title != null && p.Title.ToUpper().Contains(searchString.ToUpper()))
                   .OrderBy(o => o.Title);
                return PartialView("_HomeProducts", products);
            }
            products = _MainRepo.ProductRepository.GetAllOrderedByTitle(includeProperty: "ProductImageList,CountDownTarget");
            return PartialView("_HomeProducts", products);
        }
        #endregion
    }

}
