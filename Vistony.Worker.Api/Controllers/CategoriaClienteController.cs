using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.CategoriaCliente.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/categoria-cliente")]
    public sealed class CategoriaClienteController : ControllerBase
    {
        private readonly ObtenerCategoriaClienteUseCase _useCase;

        public CategoriaClienteController(
            ObtenerCategoriaClienteUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("CATEGORIA_CLIENTE ejecutado correctamente.");
        }
    }
}
