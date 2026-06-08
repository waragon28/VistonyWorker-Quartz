using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vistony.Worker.Application.TipoCambio.Interfaces;
using Vistony.Worker.Domain.TipoCambio;

namespace Vistony.Worker.Infrastructure.ServiceLayer
{
    public sealed class ServiceLayerTipoCambioClient : IServiceLayerTipoCambioClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ServiceLayerTipoCambioClient> _logger;

        public ServiceLayerTipoCambioClient(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ServiceLayerTipoCambioClient> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task RegistrarTipoCambioAsync(TipoCambioSunat tipoCambio)
        {
           var SessionID= await LoginAsync();

            var payload = new
            {
                Currency = "US$",
                Rate = tipoCambio.Venta,
                RateDate = tipoCambio.Fecha.ToString("yyyyMMdd")
            };

            await PostTipoCambioSapAsync(payload, SessionID);
        }

        private async Task<string> LoginAsync()
        {
            var baseUrl = _configuration["ServiceLayer:PathUri"]
                ?? throw new InvalidOperationException("No existe ServiceLayer:PathUri");

            baseUrl = baseUrl.TrimEnd('/');

            var loginUrl = $"{baseUrl}/b1s/v1/Login";

            var payload = new
            {
                CompanyDB = _configuration["ServiceLayer:PE:CompanyDB"],
                Password = _configuration["ServiceLayer:PE:Password"],
                UserName = _configuration["ServiceLayer:PE:UserName"]
            };

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var content = new StringContent(
                json,
                System.Text.Encoding.UTF8,
                "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (message, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("User-Agent", "Vistony.Worker");

            _logger.LogInformation("Ejecutando login SAP Service Layer: {Url}", loginUrl);

            var response = await client.PostAsync(loginUrl, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Error login SAP Service Layer. StatusCode: {StatusCode}, Response: {Response}",
                    response.StatusCode,
                    responseContent);

                throw new InvalidOperationException(
                    $"Error login SAP Service Layer: {responseContent}");
            }

            var jsonSession = JsonDocument.Parse(responseContent);
            var sessionId = jsonSession
                                .RootElement
                                .GetProperty("SessionId")
                                .GetString();

            _logger.LogInformation("Login SAP Service Layer correcto. Response: {Response}", responseContent);
            return sessionId ?? string.Empty;   
        }

        private async Task PostTipoCambioSapAsync(object payload,string SessionID)
        {
            var baseUrl = _configuration["ServiceLayer:PathUri"]
                ?? throw new InvalidOperationException("No existe ServiceLayer:PathUri");

            baseUrl = baseUrl.TrimEnd('/');

            var url = $"{baseUrl}/b1s/v1/SBOBobService_SetCurrencyRate";

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var content = new StringContent(
                json,
                System.Text.Encoding.UTF8,
                "application/json");

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (message, cert, chain, sslPolicyErrors) => true
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={SessionID}");

            client.DefaultRequestHeaders.Add("User-Agent", "Vistony.Worker");

            _logger.LogInformation("Registrando tipo de cambio en SAP Service Layer: {Url}", url);

            var response = await client.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Error registrando tipo de cambio en SAP. StatusCode: {StatusCode}, Response: {Response}",
                    response.StatusCode,
                    responseContent);

                throw new InvalidOperationException($"Error SAP Service Layer: {responseContent}");
            }

            _logger.LogInformation(
                "SAP respondió correctamente al registro de TC: {Response}",
                responseContent);
        }

    }
}
