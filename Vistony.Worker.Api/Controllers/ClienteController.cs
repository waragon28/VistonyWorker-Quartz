using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.Cliente.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/cliente")]
    public sealed class ClienteController : ControllerBase
    {
        private readonly ObtenerClienteUseCase _useCase;

        public ClienteController(ObtenerClienteUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("CLIENTE ejecutado correctamente.");
        }
    }
}
