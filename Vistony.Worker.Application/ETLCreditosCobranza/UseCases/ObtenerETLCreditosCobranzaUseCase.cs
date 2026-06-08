using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.ETLCreditosCobranza.Interfaces;

namespace Vistony.Worker.Application.ETLCreditosCobranza.UseCases
{
    public sealed class ObtenerETLCreditosCobranzaUseCase
    {
        private readonly IETLCreditosCobranzaRepository _repository;
        private readonly ILogger<ObtenerETLCreditosCobranzaUseCase> _logger;

        private static readonly string[] _databasesCreditosCobranza =
        {
            "B1H_VIST_PE",
            "B1H_VIST_BO",
            "B1H_VIST_CL",
            "B1H_VIST_EC",
            "B1H_VIST_ES",
            "B1H_VIST_MA",
            "B1H_VIST_PY",
            "B1H_VIST_IN"
        };

        private static readonly string[] _databasesLineaCredito =
        {
            "B1H_VIST_BO",
            "B1H_VIST_CL",
            "B1H_VIST_EC",
            "B1H_VIST_ES",
            "B1H_VIST_MA",
            "B1H_VIST_PY",
            "B1H_VIST_IN"
        };

        public ObtenerETLCreditosCobranzaUseCase(
            IETLCreditosCobranzaRepository repository,
            ILogger<ObtenerETLCreditosCobranzaUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo ETL_CREDITOS_COBRANZA");

            var tasks = new List<Task>();

            foreach (var database in _databasesCreditosCobranza)
            {
                tasks.Add(EjecutarCreditosCobranzaAsync(database));
            }

            foreach (var database in _databasesLineaCredito)
            {
                tasks.Add(EjecutarLineaCreditoAsync(database));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo ETL_CREDITOS_COBRANZA");
        }

        private async Task EjecutarCreditosCobranzaAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database, "ETL_FACT_LINEACREDITO");

                _logger.LogInformation(
                    "ETL_Creditos_Cobranza ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando ETL_Creditos_Cobranza para {Database}",
                    database);
            }
        }

        private async Task EjecutarLineaCreditoAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database, "ETL_FACT_LINEACREDITO");

                _logger.LogInformation(
                    "ETL_LineaCredito ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando ETL_LineaCredito para {Database}",
                    database);
            }
        }
    }
}
