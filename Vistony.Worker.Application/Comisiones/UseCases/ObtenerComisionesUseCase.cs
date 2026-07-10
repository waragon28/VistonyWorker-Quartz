using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Comisiones.Interfaces;
using Vistony.Worker.Application.PagoComisiones.Interfaces;

namespace Vistony.Worker.Application.Comisiones.UseCases
{
    public sealed class ObtenerComisionesUseCase
    {
        private readonly IComisionesRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<ObtenerComisionesUseCase> _logger;

        private static readonly string[] _databases =
        {
            "B1H_VIST_EC",
            "B1H_VIST_BO",
            "B1H_VIST_PY",
            "B1H_VIST_CL",
            "B1H_VIST_PE"
        };

        public ObtenerComisionesUseCase(
            IComisionesRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<ObtenerComisionesUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo COMISIONES");

            var tasks = new List<Task>();

            foreach (var database in _databases)
            {
                tasks.Add(EjecutarDatabaseAsync(database));
            }

            await Task.WhenAll(tasks);
              
            _logger.LogInformation("Finalizó flujo COMISIONES");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarAsync(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de P_VIS_VEN_COMISIONES_ACTU",
                    $"Se actualizo el P_VIS_VEN_COMISIONES_ACTU a las Hora : {DateTime.Now}");

                await _repository.EjecutarB2BAsync(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de P_VIS_VEN_PAGO_COMISIONES_B2B_ACTU",
                    $"Se actualizo el P_VIS_VEN_PAGO_COMISIONES_B2B_ACTU a las Hora : {DateTime.Now}");

                _logger.LogInformation("Comisiones ejecutado correctamente para {Database}", database);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando COMISIONES para {Database}", database);
                await _correoClient.EnviarAsync(
                    $"{database} Error Actualizacion de COMISIONES",
                    ex.Message);
            }
        }
    }
}
