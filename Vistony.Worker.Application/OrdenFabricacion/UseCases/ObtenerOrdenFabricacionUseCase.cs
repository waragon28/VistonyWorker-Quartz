using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.OrdenFabricacion.Interfaces;

namespace Vistony.Worker.Application.OrdenFabricacion.UseCases
{
    public sealed class ObtenerOrdenFabricacionUseCase
    {
        private readonly IOrdenFabricacionRepository _repository;
        private readonly IFirestoreOrdenFabricacionClient _firestoreClient;
        private readonly ILogger<ObtenerOrdenFabricacionUseCase> _logger;

        public ObtenerOrdenFabricacionUseCase(
            IOrdenFabricacionRepository repository,
            IFirestoreOrdenFabricacionClient firestoreClient,
            ILogger<ObtenerOrdenFabricacionUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo ORDEN_FABRICACION");

            var ordenes = await _repository.ObtenerNuevasAsync("B1H_VIST_PE");

            if (ordenes.Count == 0)
            {
                _logger.LogInformation("No hay nuevas órdenes para migrar.");
                return;
            }

            _logger.LogInformation("{Count} órdenes nuevas encontradas.", ordenes.Count);

            var migradasDocNums = new List<int>();

            foreach (var orden in ordenes)
            {
                try
                {
                    await _firestoreClient.GuardarAsync(orden);
                    migradasDocNums.Add(orden.OT_Envase);

                    _logger.LogInformation(
                        "OF migrada a Firestore | OT_Envase: {OT_Envase} | Item: {Item}",
                        orden.OT_Envase, orden.ItemName);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error al migrar OT_Envase {OT_Envase}. Se omite y continúa.",
                        orden.OT_Envase);
                }
            }

            if (migradasDocNums.Count > 0)
            {
                await _repository.MarcarMigradasAsync(migradasDocNums, "B1H_VIST_PE");
                _logger.LogInformation("{Count} órdenes marcadas como migradas en HANA.", migradasDocNums.Count);
            }

            _logger.LogInformation("Finalizó flujo ORDEN_FABRICACION");
        }
    }
}
