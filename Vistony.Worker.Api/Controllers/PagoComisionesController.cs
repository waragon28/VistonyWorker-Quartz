using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.PagoComisiones.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/pago-comisiones")]
    public sealed class PagoComisionesController : ControllerBase
    {
        private readonly ObtenerPagoComisionesUseCase _useCase;

        public PagoComisionesController(ObtenerPagoComisionesUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("PAGO_COMISIONES ejecutado correctamente.");
        }
    }
}
