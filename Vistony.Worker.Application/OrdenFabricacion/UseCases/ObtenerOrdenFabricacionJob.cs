using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.TipoCambio.UseCases;

namespace Vistony.Worker.Application.OrdenFabricacion.UseCases
{
    public class ObtenerOrdenFabricacionJob
    {
        private readonly ObtenerOrdenFabricacionUseCase _useCase;
        private readonly ILogger<ObtenerOrdenFabricacionJob> _logger;

        public ObtenerOrdenFabricacionJob(
            ObtenerOrdenFabricacionUseCase useCase,
            ILogger<ObtenerOrdenFabricacionJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ORDEN_FABRICACION");
            Console.WriteLine("INICIO JOB ORDEN_FABRICACION");
            await _useCase.ExecuteAsync();
            Console.WriteLine("FIN JOB ORDEN_FABRICACION");
            _logger.LogInformation("FIN JOB ORDEN_FABRICACION");
        }
    }
}
