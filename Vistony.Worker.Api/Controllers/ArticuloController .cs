using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.Articulo.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/articulo")]
    public sealed class ArticuloController : ControllerBase
    {
        private readonly ObtenerArticuloUseCase _useCase;

        public ArticuloController(
            ObtenerArticuloUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok(
                "ARTICULO ejecutado correctamente.");
        }
    }
}
