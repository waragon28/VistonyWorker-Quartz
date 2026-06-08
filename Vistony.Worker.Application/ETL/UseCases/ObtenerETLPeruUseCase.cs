using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.ETL.Interfaces;

namespace Vistony.Worker.Application.ETL.UseCases
{
    public sealed class ObtenerETLPeruUseCase
    {
        private readonly IETLRepository _repository;
        private readonly ILogger<ObtenerETLPeruUseCase> _logger;

        private const string Database = "B1H_VIST_PE";

        private static readonly string[] _procedures =
        {
            "ETL_FACT_DEUDAS",
            "ETL_FACT_DEUDAS_MOROSIDAD"
        };

        public ObtenerETLPeruUseCase(
            IETLRepository repository,
            ILogger<ObtenerETLPeruUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo ETL_PERU");

            var tasks = new List<Task>();

            foreach (var procedure in _procedures)
            {
                tasks.Add(EjecutarProcedureAsync(procedure));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo ETL_PERU");
        }

        private async Task EjecutarProcedureAsync(string procedure)
        {
            try
            {
                await _repository.EjecutarAsync(Database, procedure);

                _logger.LogInformation(
                    "{Procedure} ejecutado correctamente para {Database}",
                    procedure,
                    Database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando {Procedure} para {Database}",
                    procedure,
                    Database);
            }
        }
    }
}
