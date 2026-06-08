using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CategoriaCliente.Interfaces;

namespace Vistony.Worker.Application.CategoriaCliente.UseCases
{
    public sealed class ObtenerCategoriaClienteUseCase
    {
        private readonly ICategoriaClienteRepository _repository;
        private readonly ILogger<ObtenerCategoriaClienteUseCase> _logger;

        private const string Database = "B1H_VIST_PE";

        private static readonly string[] _procedures =
        {
            "ALERT_UPDATE_CATEGORY_CUSTOMER_HOME_CARE",
            "ALERT_UPDATE_CATEGORY_CUSTOMER_LPC",
            "ALERT_UPDATE_CATEGORY_CUSTOMER_VISTONY"
        };

        public ObtenerCategoriaClienteUseCase(
            ICategoriaClienteRepository repository,
            ILogger<ObtenerCategoriaClienteUseCase> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo CATEGORIA_CLIENTE");

            var tasks = new List<Task>();

            foreach (var procedure in _procedures)
            {
                tasks.Add(EjecutarProcedureAsync(procedure));
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Finalizó flujo CATEGORIA_CLIENTE");
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
