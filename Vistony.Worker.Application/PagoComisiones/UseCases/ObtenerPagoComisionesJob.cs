using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PagoComisiones.UseCases
{
    public class ObtenerPagoComisionesJob
    {
        private readonly ObtenerPagoComisionesUseCase _useCase;
        private readonly ILogger<ObtenerPagoComisionesJob> _logger;

        public ObtenerPagoComisionesJob(
            ObtenerPagoComisionesUseCase useCase,
            ILogger<ObtenerPagoComisionesJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB PAGO_COMISIONES");
            Console.WriteLine("INICIO JOB PAGO_COMISIONES");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB PAGO_COMISIONES");
            _logger.LogInformation("FIN JOB PAGO_COMISIONES");
        }
    }
}
