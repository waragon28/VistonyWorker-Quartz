using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Comisiones.Interfaces;
using Vistony.Worker.Application.Comisiones.UseCases;
using Vistony.Worker.Application.PagoComisiones.Interfaces;
using Vistony.Worker.Application.PCP.Interfaces;

namespace Vistony.Worker.Application.PCP.UseCase
{
    public sealed class PCPUseCase
    {
        private readonly IPCPRepository _repository;
        private readonly ICorreoPagoComisionesClient _correoClient;
        private readonly ILogger<PCPUseCase> _logger;

        private static readonly string[] _databases =
        {            
            "B1H_VIST_PE"
        };

        public PCPUseCase(
            IPCPRepository repository,
            ICorreoPagoComisionesClient correoClient,
            ILogger<PCPUseCase> logger)
        {
            _repository = repository;
            _correoClient = correoClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo PCP");
            var tasks = new List<Task>();
            foreach (var database in _databases)
            {
                tasks.Add(EjecutarDatabaseAsync(database));
            }
            await Task.WhenAll(tasks);
            _logger.LogInformation("Finalizó flujo PCP");
        }

        private async Task EjecutarDatabaseAsync(string database)
        {
            try
            {
                await _repository.EjecutarBackorder(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de BACKORDER_PCP",
                    $"Se actualizo el BACKORDER_PCP a las Hora : {DateTime.Now}");

                await _repository.EjecutarStockDiario(database);
                await _correoClient.EnviarAsync(
                    $"{database} Actualizacion de STOCK_DIARIO_PCP",
                    $"Se actualizo el STOCK_DIARIO_PCP a las Hora : {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al ejecutar PCP para la base de datos {database}");
                await _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de BACKORDER_PCP",
                    $"Ocurrió un error al actualizar el BACKORDER_PCP a las Hora : {DateTime.Now}. Error: {ex.Message}");

                _correoClient.EnviarAsync(
                    $"{database} Error en Actualizacion de STOCK_DIARIO_PCP",
                    $"Ocurrió un error al actualizar el STOCK_DIARIO_PCP a las Hora : {DateTime.Now}. Error: {ex.Message}").Wait();
            }
        }
    }
}
