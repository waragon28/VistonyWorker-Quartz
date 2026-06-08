using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.RutaHistorico.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/ruta-historico")]
    public sealed class RutaHistoricoController : ControllerBase
    {
        private readonly ObtenerRutaHistoricoUseCase _useCase;

        public RutaHistoricoController(ObtenerRutaHistoricoUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("RUTA_HISTORICO ejecutado correctamente.");
        }
    }
}
