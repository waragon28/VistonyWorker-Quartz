using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CalificacionRiesgo.UseCases
{
    public class ObtenerCalificacionRiesgoJob
    {
        private readonly ObtenerCalificacionRiesgoUseCase _useCase;
        private readonly ILogger<ObtenerCalificacionRiesgoJob> _logger;

        public ObtenerCalificacionRiesgoJob(
            ObtenerCalificacionRiesgoUseCase useCase,
            ILogger<ObtenerCalificacionRiesgoJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB CALIFICACION_RIESGO");
            Console.WriteLine("INICIO JOB CALIFICACION_RIESGO");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB CALIFICACION_RIESGO");
            _logger.LogInformation("FIN JOB CALIFICACION_RIESGO");
        }
    }
}
