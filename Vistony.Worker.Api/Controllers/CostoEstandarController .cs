using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.CostoEstandar.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/costo-estandar")]
    public sealed class CostoEstandarController : ControllerBase
    {
        private readonly ObtenerCostoEstandarUseCase _useCase;

        public CostoEstandarController(ObtenerCostoEstandarUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("COSTO_ESTANDAR ejecutado correctamente.");
        }
    }
}
