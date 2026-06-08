using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.ETLCreditosCobranza.UseCases
{
    public class ObtenerETLCreditosCobranzaJob
    {
        private readonly ObtenerETLCreditosCobranzaUseCase _useCase;
        private readonly ILogger<ObtenerETLCreditosCobranzaJob> _logger;

        public ObtenerETLCreditosCobranzaJob(
            ObtenerETLCreditosCobranzaUseCase useCase,
            ILogger<ObtenerETLCreditosCobranzaJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ETL_CREDITOS_COBRANZA");
            Console.WriteLine("INICIO JOB ETL_CREDITOS_COBRANZA");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB ETL_CREDITOS_COBRANZA");
            _logger.LogInformation("FIN JOB ETL_CREDITOS_COBRANZA");
        }
    }
}
