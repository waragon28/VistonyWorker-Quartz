using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.Comisiones.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/comisiones")]
    public sealed class ComisionesController : ControllerBase
    {
        private readonly ObtenerComisionesUseCase _useCase;

        public ComisionesController(ObtenerComisionesUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("COMISIONES ejecutado correctamente.");
        }
    }
}
