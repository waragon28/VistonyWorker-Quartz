using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.CalificacionRiesgo.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/calificacion-riesgo")]
    public sealed class CalificacionRiesgoController : ControllerBase
    {
        private readonly ObtenerCalificacionRiesgoUseCase _useCase;

        public CalificacionRiesgoController(
            ObtenerCalificacionRiesgoUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("CALIFICACION_RIESGO ejecutado correctamente.");
        }
    }
}
