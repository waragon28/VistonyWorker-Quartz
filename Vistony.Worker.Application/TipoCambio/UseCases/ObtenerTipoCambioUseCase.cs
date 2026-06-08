using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.TipoCambio.Interfaces;

namespace Vistony.Worker.Application.TipoCambio.UseCases
{
    public sealed class ObtenerTipoCambioSunatJob
    {
        private readonly SincronizarTipoCambioUseCase _useCase;
        private readonly ILogger<ObtenerTipoCambioSunatJob> _logger;

        public ObtenerTipoCambioSunatJob(
            SincronizarTipoCambioUseCase useCase,
            ILogger<ObtenerTipoCambioSunatJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB TIPO_CAMBIO");

            await _useCase.ExecuteAsync();

            _logger.LogInformation("FIN JOB TIPO_CAMBIO");
        }
    }
}