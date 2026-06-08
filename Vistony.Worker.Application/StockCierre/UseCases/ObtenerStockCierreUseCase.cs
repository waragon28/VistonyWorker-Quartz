using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.StockCierre.Interfaces;

namespace Vistony.Worker.Application.StockCierre.UseCases
{
    public sealed class ObtenerStockCierreUseCase
    {
        private readonly IStockCierreRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<ObtenerStockCierreUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_PE",
            "B1H_VIST_EC",
            "B1H_VIST_PY",
            "B1H_VIST_CL",
            "B1H_VIST_BO",
            "B1H_VIST_ES",
            "B1H_VIST_MA"
        };

        public ObtenerStockCierreUseCase(
            IStockCierreRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<ObtenerStockCierreUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo STOCK_CIERRE");

            var tasks = _databases
                .Select(database => EjecutarDatabaseAsync(database));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo STOCK_CIERRE");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database);

                await _correoClient.EnviarAsync(
                    $"{database} P_VIS_STOCK_CIERRE",
                    $"Se actualizó correctamente : {DateTime.Now}");

                _logger.LogInformation(
                    "Stock cierre ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando STOCK_CIERRE para {Database}",
                    database);
            }
        }
    }
}
