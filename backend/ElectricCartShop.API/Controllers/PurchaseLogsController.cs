using ElectricCartShop.API.Interfaces;
using ElectricCartShop.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectricCartShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseLogsController : ControllerBase
    {
        private readonly IPurchaseLogService _purchaseLogService;

        public PurchaseLogsController(IPurchaseLogService purchaseLogService)
        {
            _purchaseLogService = purchaseLogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseLog>>> GetPurchaseLogs()
        {
            var logs = await _purchaseLogService.GetAllAsync();
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseLog>> GetPurchaseLog(int id)
        {
            var log = await _purchaseLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        [HttpGet("order/{orderNumber}")]
        public async Task<ActionResult<IEnumerable<PurchaseLog>>> GetPurchaseLogsByOrder(string orderNumber)
        {
            var logs = await _purchaseLogService.GetByOrderNumberAsync(orderNumber);
            return Ok(logs);
        }
    }
}


