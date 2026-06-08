using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CuentaSalesForce.Interfaces;

namespace Vistony.Worker.Infrastructure.SalesForce
{
    public class SalesForceCuentaClient : ISalesForceCuentaClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SalesForceCuentaClient(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task ActualizarCuentaAsync(
            Domain.CuentaSalesForce.CuentaSalesForce cuenta)
        {
            string token = await LoginAsync();

            var payload = new
            {
                Estado_Aproacion_Linea_de_credito__c = "Aprobado",
                Condicion_de_Pago__c = $"PE {cuenta.GroupNum}",
                Linea_Credito__c = cuenta.CreditLine
            };

            string baseUrl = _configuration["SalesForce:BaseUrl"]!;
            string url = $"{baseUrl}/services/data/v56.0/sobjects/Account/{cuenta.CodigoSalesForce}";

            string json = JsonConvert.SerializeObject(payload);

            using var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await _httpClient.SendAsync(request);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Error Salesforce PATCH | StatusCode: {response.StatusCode} | Response: {responseContent}");
            }
        }

        private async Task<string> LoginAsync()
        {
            string loginUrl = _configuration["SalesForce:LoginUrl"]!;
            string clientId = _configuration["SalesForce:ClientId"]!;
            string clientSecret = _configuration["SalesForce:ClientSecret"]!;
            string username = _configuration["SalesForce:Username"]!;
            string password = _configuration["SalesForce:Password"]!;

            var values = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "username", username },
                { "password", password }
            };

            using var content = new FormUrlEncodedContent(values);

            using HttpResponseMessage response = await _httpClient.PostAsync(loginUrl, content);

            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Error login Salesforce | StatusCode: {response.StatusCode} | Response: {responseContent}");
            }

            JObject json = JObject.Parse(responseContent);

            return json["access_token"]?.ToString() ?? string.Empty;
        }
    }
}
