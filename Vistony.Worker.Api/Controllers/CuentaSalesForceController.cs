using Microsoft.AspNetCore.Mvc;
using Vistony.Worker.Application.CuentaSalesForce.UseCases;

namespace Vistony.Worker.Api.Controllers
{
    [ApiController]
    [Route("api/cuenta-salesforce")]
    public sealed class CuentaSalesForceController : ControllerBase
    {
        private readonly ObtenerCuentaSalesForceUseCase _useCase;

        public CuentaSalesForceController(ObtenerCuentaSalesForceUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("ejecutar")]
        public async Task<IActionResult> Ejecutar()
        {
            await _useCase.ExecuteAsync();

            return Ok("CUENTA_SALESFORCE ejecutado correctamente.");
        }
    }
}
