using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.StockCierre.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/stock-cierre")]
    public sealed class StockCierreController : ControllerBase
    {
        private readonly ObtenerStockCierreUseCase _useCase;

        public StockCierreController(
            ObtenerStockCierreUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok(
                "STOCK_CIERRE ejecutado correctamente.");
        }
    }
}
