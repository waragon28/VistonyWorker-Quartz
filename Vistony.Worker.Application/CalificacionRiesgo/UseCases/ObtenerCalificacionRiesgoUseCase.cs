using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CalificacionRiesgo.Interfaces;

namespace Vistony.Worker.Application.CalificacionRiesgo.UseCases
{
    public sealed class ObtenerCalificacionRiesgoUseCase
    {
        private readonly ICalificacionRiesgoRepository _repository;
        private readonly ILogger<ObtenerCalificacionRiesgoUseCase> _logger;

        private const string Database = "B1H_VIST_PE";

        private static readonly string[] _procedures =
        {
            "APP_UPDATE_RISK_CALIFICATION_WITHOUT_DEBT",
            "ALERT_UPDATE_CATEGORY_CUSTOMER_LPC",
            "APP_UPDATE_RISK_CALIFICATION_WITH_DEBT"
        };

        public ObtenerCalificacionRiesgoUseCase(
            ICalificacionRiesgoRepository repository,
            ILogger<ObtenerCalificacionRiesgoUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo CALIFICACION_RIESGO");

            var tasks = new List<Task>();

            foreach (var procedure in _procedures)
            {
                tasks.Add(EjecutarProcedureAsync(procedure));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo CALIFICACION_RIESGO");
        }

        private async Task EjecutarProcedureAsync(string procedure)
        {
            try
            {
                await _repository.EjecutarAsync(Database, procedure);

                _logger.LogInformation(
                    "Procedimiento {Procedure} ejecutado correctamente.",
                    procedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando procedimiento {Procedure}.",
                    procedure);
            }
        }
    }
}
