using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.ETL.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/etl-peru")]
    public sealed class ETLPeruController : ControllerBase
    {
        private readonly ObtenerETLPeruUseCase _useCase;

        public ETLPeruController(ObtenerETLPeruUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("ETL_PERU ejecutado correctamente.");
        }
    }
}
