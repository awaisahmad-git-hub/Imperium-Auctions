using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ImperiumAuctions.DTOs;
using ImperiumAuctions.Repository.IRepository;
using ImperiumAuctions.Utility;

namespace ImperiumAuctions.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles = StaticValues.Role_Admin)]
    public class ManageOrderController : Controller
    {
        private readonly IMainRepository _MainRepo;
        public ManageOrderController(IMainRepository MainRepo)
        {
            _MainRepo = MainRepo;
        }
        [HttpGet("admin/manage-orders")]
        public IActionResult Index()
        {
            var auctionOrders = _MainRepo.AuctionOrderRepository.GetAllOrderedByDeliveryStatus();
            return View(auctionOrders);
        }

        #region Api methods
        [HttpPost]
        public async Task<IActionResult> StatusShipped([FromBody] OrderIdDTO orderIdDTO)
        {
            _MainRepo.AuctionOrderRepository.UpdateStatus(orderIdDTO.OrderId, StaticValues.DeliveryStatusShipped);
            await _MainRepo.AuctionOrderRepository.SaveA();

            var orders = _MainRepo.AuctionOrderRepository.GetAllOrderedByDeliveryStatus();
            return PartialView("_ManageOrderTableBody", orders);

        }
        [HttpPost]
        public async Task<IActionResult> StatusDelivered([FromBody] OrderIdDTO orderIdDTO)
        {
            _MainRepo.AuctionOrderRepository.UpdateStatus(orderIdDTO.OrderId, StaticValues.DeliveryStatusDelivered);
            await _MainRepo.AuctionOrderRepository.SaveA();

            var orders = _MainRepo.AuctionOrderRepository.GetAllOrderedByDeliveryStatus();
            return PartialView("_ManageOrderTableBody", orders);
        }
        #endregion
    }
}
