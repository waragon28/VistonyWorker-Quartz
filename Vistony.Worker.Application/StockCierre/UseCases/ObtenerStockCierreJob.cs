using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.StockCierre.UseCases
{
    public class ObtenerStockCierreJob
    {
        private readonly ObtenerStockCierreUseCase _useCase;
        private readonly ILogger<ObtenerStockCierreJob> _logger;

        public ObtenerStockCierreJob(
            ObtenerStockCierreUseCase useCase,
            ILogger<ObtenerStockCierreJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB STOCK_CIERRE");

            await _useCase.ExecuteAsync();

            _logger.LogInformation("FIN JOB STOCK_CIERRE");
        }
    }
}
