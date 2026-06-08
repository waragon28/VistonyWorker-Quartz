using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.ETL.Interfaces;

namespace Vistony.Worker.Application.ETL.UseCases
{
    public sealed class ObtenerETLInduvisUseCase
    {
        private readonly IETLInduvisRepository _repository;
        private readonly ILogger<ObtenerETLInduvisUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_EC",
            "B1H_VIST_PY",
            "B1H_VIST_ES",
            "B1H_VIST_MA",
            "B1H_VIST_CL",
            "B1H_VIST_BO",
            "B1H_VIST_IN"
        };

        public ObtenerETLInduvisUseCase(
        IETLInduvisRepository repository,       
        ILogger<ObtenerETLInduvisUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo ETL_INDUVIS");

            var tasks = new List<Task>();

            foreach (var database in _databases)
            {
                tasks.Add(EjecutarDatabaseAsync(database));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo ETL_INDUVIS");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database, "ETL_FACT_DEUDAS");

                _logger.LogInformation(
                    "ETL_FACT_DEUDAS ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando ETL_FACT_DEUDAS para {Database}",
                    database);
            }
        }
    }
}
