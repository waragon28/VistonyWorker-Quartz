using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.TipoCambio.Interfaces;

namespace Vistony.Worker.Infrastructure.TipoCambio
{
    public sealed class TipoCambioFechaHanaRepository : ITipoCambioFechaRepository
    {
        private readonly string _connectionString;
        string BD = string.Empty;
        public TipoCambioFechaHanaRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")
                ?? configuration.GetConnectionString("HanaConnectionSEIDOR")
                ?? throw new InvalidOperationException("No se encontró una cadena de conexión válida.");

            BD = configuration["ServiceLayer:PE:CompanyDB"]
                ?? throw new InvalidOperationException("No se encontró ServiceLayer:PE:CompanyDB.");
        }

        public async Task<DateTime> ObtenerFechaTipoCambioAsync()
        {
            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = $"CALL {BD}.SP_OBTENER_FECHA_TC_PE()";

            var result = await command.ExecuteScalarAsync();

            if (result is null || result == DBNull.Value)
                throw new InvalidOperationException("El SP SP_OBTENER_FECHA_TC_PE no devolvió fecha.");

            return Convert.ToDateTime(result);
        }
    }
}
