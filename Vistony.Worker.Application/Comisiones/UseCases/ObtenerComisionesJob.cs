using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Comisiones.UseCases
{
    public class ObtenerComisionesJob
    {
        private readonly ObtenerComisionesUseCase _useCase;
        private readonly ILogger<ObtenerComisionesJob> _logger;

        public ObtenerComisionesJob(
            ObtenerComisionesUseCase useCase,
            ILogger<ObtenerComisionesJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB COMISIONES");
            Console.WriteLine("INICIO JOB COMISIONES");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB COMISIONES");
            _logger.LogInformation("FIN JOB COMISIONES");
        }
    }
}
