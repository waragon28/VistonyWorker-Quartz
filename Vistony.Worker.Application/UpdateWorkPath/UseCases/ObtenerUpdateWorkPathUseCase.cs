using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.UpdateWorkPath.Interfaces;

namespace Vistony.Worker.Application.UpdateWorkPath.UseCases
{
    public sealed class ObtenerUpdateWorkPathUseCase
    {
        private readonly IUpdateWorkPathRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<ObtenerUpdateWorkPathUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_PE",
            "B1H_ROFA_PE",
            "B1H_VIST_CL",
            "B1H_VIST_BO",
            "B1H_VIST_EC",
            "B1H_VIST_PY"
        };

        public ObtenerUpdateWorkPathUseCase(
            IUpdateWorkPathRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<ObtenerUpdateWorkPathUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo UPDATE_WORKPATH");

            var tasks = _databases
                .Select(database => EjecutarDatabaseAsync(database));

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo UPDATE_WORKPATH");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database);

                await _correoClient.EnviarAsync(
                    $"{database} UPDATE_WORKPATH",
                    "Se Actualizo la Ruta");

                _logger.LogInformation(
                    "Update WorkPath ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando UPDATE_WORKPATH para {Database}",
                    database);
            }
        }
    }
}
