using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.ETLCreditosCobranza.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/etl-creditos-cobranza")]
    public sealed class ETLCreditosCobranzaController : ControllerBase
    {
        private readonly ObtenerETLCreditosCobranzaUseCase _useCase;

        public ETLCreditosCobranzaController(
            ObtenerETLCreditosCobranzaUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("ETL_CREDITOS_COBRANZA ejecutado correctamente.");
        }
    }
}
