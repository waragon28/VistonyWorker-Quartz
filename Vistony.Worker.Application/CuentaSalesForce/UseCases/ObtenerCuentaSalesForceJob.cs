using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CuentaSalesForce.UseCases
{
    public class ObtenerCuentaSalesForceJob
    {
        private readonly ObtenerCuentaSalesForceUseCase _useCase;
        private readonly ILogger<ObtenerCuentaSalesForceJob> _logger;

        public ObtenerCuentaSalesForceJob(
            ObtenerCuentaSalesForceUseCase useCase,
            ILogger<ObtenerCuentaSalesForceJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB CUENTA_SALESFORCE");
            Console.WriteLine("INICIO JOB CUENTA_SALESFORCE");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB CUENTA_SALESFORCE");
            _logger.LogInformation("FIN JOB CUENTA_SALESFORCE");
        }
    }
}
