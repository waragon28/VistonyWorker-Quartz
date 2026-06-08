using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.ETL.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/etl-induvis")]
    public sealed class ETLInduvisController : ControllerBase
    {
        private readonly ObtenerETLInduvisUseCase _useCase;

        public ETLInduvisController(ObtenerETLInduvisUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("ETL_INDUVIS ejecutado correctamente.");
        }
    }
}
