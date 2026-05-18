using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyAPI.Services;

namespace SmartEnergyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EnergyProductController : ControllerBase
    {
        private readonly IEnergyProductService _energyProductService;

        public EnergyProductController(IEnergyProductService energyProductService)
        {
            _energyProductService = energyProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _energyProductService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _energyProductService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost("seed")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Seed()
        {
            await _energyProductService.SeedEnergyProductsAsync();
            return Ok(new { message = "Energiprodukter er tilføjet." });
        }

        [HttpPost("update-prices")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePrices()
        {
            await _energyProductService.UpdatePricesFromEnergidataAsync();
            return Ok(new { message = "Priserne er opdateret." });
        }
    }
}
