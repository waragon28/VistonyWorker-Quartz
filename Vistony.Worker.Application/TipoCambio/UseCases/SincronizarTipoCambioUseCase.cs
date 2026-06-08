using Microsoft.Extensions.Logging;
using Vistony.Worker.Application.TipoCambio.Interfaces;

namespace Vistony.Worker.Application.TipoCambio.UseCases
{
    public sealed class SincronizarTipoCambioUseCase
    {
        private readonly ITipoCambioFechaRepository _fechaRepository;
        private readonly ISunatTipoCambioClient _sunatClient;
        private readonly IServiceLayerTipoCambioClient _serviceLayerClient;
        private readonly ILogger<SincronizarTipoCambioUseCase> _logger;

        public SincronizarTipoCambioUseCase(
            ITipoCambioFechaRepository fechaRepository,
            ISunatTipoCambioClient sunatClient,
            IServiceLayerTipoCambioClient serviceLayerClient,
            ILogger<SincronizarTipoCambioUseCase> logger)
        {
            _fechaRepository = fechaRepository;
            _sunatClient = sunatClient;
            _serviceLayerClient = serviceLayerClient;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Iniciando flujo TIPO_CAMBIO");

            var fecha = await _fechaRepository.ObtenerFechaTipoCambioAsync();

            _logger.LogInformation(
                "Fecha obtenida desde HANA: {Fecha}",
                fecha.ToString("yyyy-MM-dd"));

            var limite = DateTime.Today.AddHours(12);

            while (DateTime.Now < limite)
            {
                try
                {
                    _logger.LogInformation(
                        "Consultando API SUNAT para fecha {Fecha}",
                        fecha.ToString("yyyy-MM-dd"));

                    var tipoCambio = await _sunatClient.ObtenerAsync(fecha);

                    if (tipoCambio is null || tipoCambio.Venta <= 0)
                    {
                        _logger.LogWarning(
                            "SUNAT aún no tiene TC válido para {Fecha}. Reintentando en 10 minutos.",
                            fecha.ToString("yyyy-MM-dd"));

                        await Task.Delay(TimeSpan.FromMinutes(10));
                        continue;
                    }

                    _logger.LogInformation(
                        "TC obtenido SUNAT | Fecha: {Fecha} | Compra: {Compra} | Venta: {Venta} | Moneda: {Moneda}",
                        tipoCambio.Fecha.ToString("yyyy-MM-dd"),
                        tipoCambio.Compra,
                        tipoCambio.Venta,
                        tipoCambio.Moneda);

                    await _serviceLayerClient.RegistrarTipoCambioAsync(tipoCambio);

                    _logger.LogInformation(
                        "TC registrado correctamente en SAP | Fecha: {Fecha} | Venta: {Venta}",
                        tipoCambio.Fecha.ToString("yyyy-MM-dd"),
                        tipoCambio.Venta);

                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error en flujo TIPO_CAMBIO. Reintentando en 10 minutos.");

                    await Task.Delay(TimeSpan.FromMinutes(10));
                }
            }

            _logger.LogInformation("Finalizó flujo TIPO_CAMBIO");
        }
    }
}