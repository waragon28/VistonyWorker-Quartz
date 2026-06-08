using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CostoEstandar.UseCases
{
    public class ObtenerCostoEstandarJob
    {
        private readonly ObtenerCostoEstandarUseCase _useCase;
        private readonly ILogger<ObtenerCostoEstandarJob> _logger;

        public ObtenerCostoEstandarJob(
            ObtenerCostoEstandarUseCase useCase,
            ILogger<ObtenerCostoEstandarJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB COSTO_ESTANDAR");
            Console.WriteLine("INICIO JOB COSTO_ESTANDAR");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB COSTO_ESTANDAR");
            _logger.LogInformation("FIN JOB COSTO_ESTANDAR");
        }
    }
}
