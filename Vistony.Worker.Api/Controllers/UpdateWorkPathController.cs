using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.UpdateWorkPath.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/update-workpath")]
    public sealed class UpdateWorkPathController : ControllerBase
    {
        private readonly ObtenerUpdateWorkPathUseCase _useCase;

        public UpdateWorkPathController(ObtenerUpdateWorkPathUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();
            return Ok("UPDATE_WORKPATH ejecutado correctamente.");
        }
    }
}
