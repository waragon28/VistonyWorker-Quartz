using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.UpdateWorkPath.UseCases
{
    public class ObtenerUpdateWorkPathJob
    {
        private readonly ObtenerUpdateWorkPathUseCase _useCase;
        private readonly ILogger<ObtenerUpdateWorkPathJob> _logger;

        public ObtenerUpdateWorkPathJob(
            ObtenerUpdateWorkPathUseCase useCase,
            ILogger<ObtenerUpdateWorkPathJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB UPDATE_WORKPATH");
            Console.WriteLine("INICIO JOB UPDATE_WORKPATH");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB UPDATE_WORKPATH");
            _logger.LogInformation("FIN JOB UPDATE_WORKPATH");
        }
    }
}
