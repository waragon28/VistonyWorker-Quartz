using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Cliente.UseCases
{
    public class ObtenerClienteJob
    {
        private readonly ObtenerClienteUseCase _useCase;
        private readonly ILogger<ObtenerClienteJob> _logger;

        public ObtenerClienteJob(
            ObtenerClienteUseCase useCase,
            ILogger<ObtenerClienteJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB CLIENTE");
            Console.WriteLine("INICIO JOB CLIENTE");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB CLIENTE");
            _logger.LogInformation("FIN JOB CLIENTE");
        }
    }
}
