using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.RutaHistorico.Interfaces;

namespace Vistony.Worker.Application.RutaHistorico.UseCases
{
    public sealed class ObtenerRutaHistoricoUseCase
    {
        private readonly IRutaHistoricoRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<ObtenerRutaHistoricoUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_PE",
            "B1H_ROFA_PE",
            "B1H_VIST_CL",
            "B1H_VIST_BO",
            "B1H_VIST_EC",
            "B1H_VIST_PY"
        };

        public ObtenerRutaHistoricoUseCase(
            IRutaHistoricoRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<ObtenerRutaHistoricoUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo RUTA_HISTORICO");

            var tasks = _databases
                .Select(database => EjecutarDatabaseAsync(database));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo RUTA_HISTORICO");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database);

                await _correoClient.EnviarAsync(
                    $"{database} P_VIS_VEN_HOJA_RUTA_HISTORICO",
                    "Se Actualizo la Ruta Historica");

                _logger.LogInformation(
                    "Ruta histórica ejecutada correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando Ruta Historica para {Database}",
                    database);
            }
        }
    }
}
