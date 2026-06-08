using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Cliente.Interfaces;

namespace Vistony.Worker.Application.Cliente.UseCases
{
    public sealed class ObtenerClienteUseCase
    {
        private readonly IClienteRepository _repository;
        private readonly IFirestoreClienteClient _firestoreClient;
        private readonly ILogger<ObtenerClienteUseCase> _logger;

        public ObtenerClienteUseCase(
            IClienteRepository repository,
            IFirestoreClienteClient firestoreClient,
            ILogger<ObtenerClienteUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            const string database = "B1H_VIST_PE";

            _logger.LogInformation("Iniciando flujo CLIENTE");

            var clientes = await _repository.ObtenerNuevosAsync(database);

            if (clientes.Count == 0)
            {
                _logger.LogInformation("No hay nuevos clientes para migrar.");
                return;
            }

            _logger.LogInformation("{Count} clientes nuevos encontrados.", clientes.Count);

            var migradosCardCodes = new List<string>();

            foreach (var cliente in clientes)
            {
                try
                {
                    var existe = await _firestoreClient.ExisteAsync(cliente.CodeSap);

                    if (existe)
                        continue;

                    await _firestoreClient.GuardarAsync(cliente);
                    migradosCardCodes.Add(cliente.CardCode);

                    _logger.LogInformation(
                        "Cliente migrado a Firestore | CodeSap: {CodeSap} | CardCode: {CardCode}",
                        cliente.CodeSap,
                        cliente.CardCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error al migrar cliente {CardCode}. Se omite y continúa.",
                        cliente.CardCode);
                }
            }

            if (migradosCardCodes.Count > 0)
            {
                await _repository.MarcarMigradosAsync(migradosCardCodes, database);

                _logger.LogInformation(
                    "{Count} clientes marcados como migrados en HANA.",
                    migradosCardCodes.Count);
            }

            _logger.LogInformation("Finalizó flujo CLIENTE");
        }
    }
}
