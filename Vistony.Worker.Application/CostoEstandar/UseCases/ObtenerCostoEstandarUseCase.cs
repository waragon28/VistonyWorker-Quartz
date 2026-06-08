using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CostoEstandar.Interfaces;

namespace Vistony.Worker.Application.CostoEstandar.UseCases
{
    public sealed class ObtenerCostoEstandarUseCase
    {
        private readonly ICostoEstandarRepository _repository;
        private readonly ILogger<ObtenerCostoEstandarUseCase> _logger;

        public ObtenerCostoEstandarUseCase(
            ICostoEstandarRepository repository,
            ILogger<ObtenerCostoEstandarUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            const string database = "B1H_VIST_PE";

            _logger.LogInformation("Iniciando flujo COSTO_ESTANDAR");

            try
            {
                await _repository.EjecutarAsync(database);

                _logger.LogInformation(
                    "Procedimiento COSTO_ESTANDAR ejecutado correctamente para {Database}",
                    database);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error ejecutando COSTO_ESTANDAR para {Database}",
                    database);

                throw;
            }

            _logger.LogInformation("Finalizó flujo COSTO_ESTANDAR");
        }
    }
}
