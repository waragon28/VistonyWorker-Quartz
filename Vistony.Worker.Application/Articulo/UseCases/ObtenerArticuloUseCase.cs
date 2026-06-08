using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Articulo.Interfaces;

namespace Vistony.Worker.Application.Articulo.UseCases
{
    public sealed class ObtenerArticuloUseCase
    {
        private readonly IArticuloRepository _repository;
        private readonly IFirestoreArticuloClient _firestoreClient;
        private readonly ILogger<ObtenerArticuloUseCase> _logger;

        private static readonly string[] _locations =
        {
        "PE",
        "CL",
        "EC",
        "PY",
        "BO"
    };

        public ObtenerArticuloUseCase(
            IArticuloRepository repository,
            IFirestoreArticuloClient firestoreClient,
            ILogger<ObtenerArticuloUseCase> logger)
        {
            _repository = repository;
            _firestoreClient = firestoreClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            foreach (var location in _locations)
            {
                try
                {
                    var companyId =
                        await _firestoreClient.ObtenerCompanyIdAsync(location);

                    if (string.IsNullOrWhiteSpace(companyId))
                        continue;

                    var itemCodes =
                        await _repository.ObtenerCodigosAsync(location);

                    foreach (var itemCode in itemCodes)
                    {
                        try
                        {
                            bool existe =
                                await _firestoreClient.ExisteAsync(
                                    companyId,
                                    itemCode);

                            if (existe)
                                continue;

                            var articulo =
                                await _repository.ObtenerDetalleAsync(
                                    location,
                                    companyId,
                                    itemCode);

                            if (articulo == null)
                                continue;

                            await _firestoreClient.GuardarAsync(articulo);

                            _logger.LogInformation(
                                "Artículo migrado {ItemCode}",
                                itemCode);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Error ItemCode {ItemCode}",
                                itemCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error Location {Location}",
                        location);
                }
            }
        }
    }
}
