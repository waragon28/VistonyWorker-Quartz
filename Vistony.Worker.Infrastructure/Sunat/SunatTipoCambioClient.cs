using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using Vistony.Worker.Application.TipoCambio.Interfaces;
using Vistony.Worker.Domain.TipoCambio;

namespace Vistony.Worker.Infrastructure.Sunat
{
    public sealed class SunatTipoCambioClient : ISunatTipoCambioClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<SunatTipoCambioClient> _logger;

        public SunatTipoCambioClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<SunatTipoCambioClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["Sunat:TipoCambioUrl"]
                ?? throw new InvalidOperationException("No existe Sunat:TipoCambioUrl");
        }

        public async Task<TipoCambioSunat?> ObtenerAsync(DateTime fecha)
        {
            var fechaTexto = fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var url = $"{_baseUrl}?fecha={fechaTexto}";

            _logger.LogInformation("Consultando SUNAT: {Url}", url);

            try
            {
                var httpResponse = await _httpClient.GetAsync(url);
                var content = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "SUNAT no respondió OK. StatusCode: {StatusCode}, Response: {Response}",
                        httpResponse.StatusCode,
                        content);

                    return null;
                }

                if (string.IsNullOrWhiteSpace(content))
                {
                    _logger.LogWarning(
                        "SUNAT devolvió respuesta vacía para fecha {Fecha}",
                        fechaTexto);

                    return null;
                }

                TipoCambioSunat? tipoCambio;

                try
                {
                    tipoCambio = JsonSerializer.Deserialize<TipoCambioSunat>(
                        content,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(
                        ex,
                        "SUNAT devolvió formato inválido para fecha {Fecha}. Response: {Response}",
                        fechaTexto,
                        content);

                    return null;
                }

                if (tipoCambio is null)
                {
                    _logger.LogWarning(
                        "No se pudo interpretar la respuesta SUNAT para fecha {Fecha}. Response: {Response}",
                        fechaTexto,
                        content);

                    return null;
                }

                if (tipoCambio.Venta <= 0)
                {
                    _logger.LogWarning(
                        "SUNAT devolvió TC no válido para fecha {Fecha}. Venta: {Venta}",
                        fechaTexto,
                        tipoCambio.Venta);

                    return null;
                }

                return tipoCambio;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Error HTTP consultando SUNAT para fecha {Fecha}",
                    fechaTexto);

                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(
                    ex,
                    "Timeout consultando SUNAT para fecha {Fecha}",
                    fechaTexto);

                return null;
            }
        }
    }
}