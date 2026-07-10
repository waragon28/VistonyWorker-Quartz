using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.PCP.UseCase;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/pcp")]
    public sealed class PCPController : ControllerBase
    {
        private readonly PCPUseCase _useCase;

        public PCPController(PCPUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("PCP ejecutado correctamente.");
        }
    }
}
