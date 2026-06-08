using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.OrdenFabricacion.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/orden-fabricacion")]
    public sealed class OrdenFabricacionController : ControllerBase
    {
        private readonly ObtenerOrdenFabricacionUseCase _useCase;

        public OrdenFabricacionController(ObtenerOrdenFabricacionUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("ORDEN_FABRICACION ejecutado correctamente.");
        }
    }
}
