using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Almacen.Interfaces;

namespace Vistony.Worker.Application.Almacen.UseCases
{
    public sealed class ObtenerAlmacenUseCase
    {
        private readonly IAlmacenRepository _repository;
        private readonly IFirestoreAlmacenClient _firestoreClient;
        private readonly ILogger<ObtenerAlmacenUseCase> _logger;

        private static readonly string[] _locations = { "PE", "CL", "EC", "PY", "BO" };

        public ObtenerAlmacenUseCase(
            IAlmacenRepository repository,
            IFirestoreAlmacenClient firestoreClient,
            ILogger<ObtenerAlmacenUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo ALMACEN");

            foreach (var location in _locations)
            {
                try
                {
                    _logger.LogInformation("Procesando almacenes para {Location}", location);

                    var companyId = await _firestoreClient.ObtenerCompanyIdAsync(location);

                    if (string.IsNullOrWhiteSpace(companyId))
                    {
                        _logger.LogWarning("No se encontró companyId en Firestore para {Location}", location);
                        continue;
                    }

                    var almacenes = await _repository.ObtenerAlmacenesAsync(location, companyId);

                    if (almacenes.Count == 0)
                    {
                        _logger.LogInformation("No hay almacenes para migrar en {Location}", location);
                        continue;
                    }

                    var migrados = 0;

                    foreach (var almacen in almacenes)
                    {
                        try
                        {
                            var existe = await _firestoreClient.ExisteAsync(
                                almacen.CompanyId,
                                almacen.WarehouseCode);

                            if (existe)
                                continue;

                            await _firestoreClient.GuardarAsync(almacen);
                            migrados++;

                            _logger.LogInformation(
                                "Almacén migrado | Location: {Location} | WarehouseCode: {WarehouseCode}",
                                location,
                                almacen.WarehouseCode);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex,
                                "Error al migrar almacén {WarehouseCode} para {Location}. Se omite y continúa.",
                                almacen.WarehouseCode,
                                location);
                        }
                    }

                    _logger.LogInformation(
                        "{Count} almacenes migrados para {Location}",
                        migrados,
                        location);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error general migrando almacenes para {Location}",
                        location);
                }
            }

            _logger.LogInformation("Finalizó flujo ALMACEN");
        }
    }
}
