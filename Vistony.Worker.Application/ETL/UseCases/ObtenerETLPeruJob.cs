using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.ETL.UseCases
{
    public class ObtenerETLPeruJob
    {
        private readonly ObtenerETLPeruUseCase _useCase;
        private readonly ILogger<ObtenerETLPeruJob> _logger;

        public ObtenerETLPeruJob(
            ObtenerETLPeruUseCase useCase,
            ILogger<ObtenerETLPeruJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ETL_PERU");
            Console.WriteLine("INICIO JOB ETL_PERU");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB ETL_PERU");
            _logger.LogInformation("FIN JOB ETL_PERU");
        }
    }
}
