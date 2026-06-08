using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.TipoCambio.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/tipo-cambio")]
    public sealed class TipoCambioController : ControllerBase
    {
        private readonly SincronizarTipoCambioUseCase _useCase;

        public TipoCambioController(SincronizarTipoCambioUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("TIPO_CAMBIO ejecutado correctamente.");
        }
    }
}
