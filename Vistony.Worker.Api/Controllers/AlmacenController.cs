using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.Almacen.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/almacen")]
    public sealed class AlmacenController : ControllerBase
    {
        private readonly ObtenerAlmacenUseCase _useCase;

        public AlmacenController(ObtenerAlmacenUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("ALMACEN ejecutado correctamente.");
        }
    }
}
