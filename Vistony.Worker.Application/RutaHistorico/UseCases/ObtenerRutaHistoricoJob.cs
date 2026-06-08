using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.RutaHistorico.UseCases
{
    public class ObtenerRutaHistoricoJob
    {
        private readonly ObtenerRutaHistoricoUseCase _useCase;
        private readonly ILogger<ObtenerRutaHistoricoJob> _logger;

        public ObtenerRutaHistoricoJob(
            ObtenerRutaHistoricoUseCase useCase,
            ILogger<ObtenerRutaHistoricoJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB RUTA_HISTORICO");
            Console.WriteLine("INICIO JOB RUTA_HISTORICO");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB RUTA_HISTORICO");
            _logger.LogInformation("FIN JOB RUTA_HISTORICO");
        }
    }
}
