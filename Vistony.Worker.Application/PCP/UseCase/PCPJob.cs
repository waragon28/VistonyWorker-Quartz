using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PCP.UseCase
{
    public class PCPJob
    {
        private readonly PCPUseCase _useCase;
        private readonly ILogger<PCPJob> _logger;

        public PCPJob(
            PCPUseCase useCase,
            ILogger<PCPJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB PCP");
            Console.WriteLine("INICIO JOB PCP");
            await _useCase.ExecuteAsync();
            Console.WriteLine("FIN JOB PCP");
            _logger.LogInformation("FIN JOB PCP");
        }
    }
}
