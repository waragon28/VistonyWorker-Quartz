using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CuentaSalesForce.Interfaces;

namespace Vistony.Worker.Application.CuentaSalesForce.UseCases
{
    public sealed class ObtenerCuentaSalesForceUseCase
    {
        private readonly ICuentaSalesForceRepository _repository;
        private readonly ISalesForceCuentaClient _salesForceClient;
        private readonly ILogger<ObtenerCuentaSalesForceUseCase> _logger;

        public ObtenerCuentaSalesForceUseCase(
            ICuentaSalesForceRepository repository,
            ISalesForceCuentaClient salesForceClient,
            ILogger<ObtenerCuentaSalesForceUseCase> logger)
        {
            _repository = repository;
            _salesForceClient = salesForceClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            const string database = "B1H_VIST_PE";

            _logger.LogInformation("Iniciando flujo CUENTA_SALESFORCE");

            var cuentas = await _repository.ObtenerPendientesAsync(database);

            if (cuentas.Count == 0)
            {
                _logger.LogInformation("No hay cuentas pendientes para actualizar en Salesforce.");
                return;
            }

            _logger.LogInformation("{Count} cuentas encontradas para actualizar.", cuentas.Count);

            foreach (var cuenta in cuentas)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(cuenta.CodigoSalesForce))
                        continue;

                    await _salesForceClient.ActualizarCuentaAsync(cuenta);

                    _logger.LogInformation(
                        "Cuenta Salesforce actualizada | CodigoSalesForce: {CodigoSalesForce}",
                        cuenta.CodigoSalesForce);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error actualizando cuenta Salesforce {CodigoSalesForce}. Se omite y continúa.",
                        cuenta.CodigoSalesForce);
                }
            }

            _logger.LogInformation("Finalizó flujo CUENTA_SALESFORCE");
        }
    }
}
