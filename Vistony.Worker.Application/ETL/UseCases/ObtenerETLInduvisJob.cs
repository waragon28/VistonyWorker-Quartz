using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.ETL.UseCases
{
    public class ObtenerETLInduvisJob
    {
        private readonly ObtenerETLInduvisUseCase _useCase;
        private readonly ILogger<ObtenerETLInduvisJob> _logger;

        public ObtenerETLInduvisJob(
            ObtenerETLInduvisUseCase useCase,
            ILogger<ObtenerETLInduvisJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ETL_INDUVIS");
            Console.WriteLine("INICIO JOB ETL_INDUVIS");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB ETL_INDUVIS");
            _logger.LogInformation("FIN JOB ETL_INDUVIS");
        }
    }
}
