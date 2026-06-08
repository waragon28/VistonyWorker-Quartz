using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Almacen.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class AlmacenHanaRepository : IAlmacenRepository
    {
        private readonly string _connectionString;

        public AlmacenHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task<List<Domain.Almacen.Almacen>> ObtenerAlmacenesAsync(string location, string companyId)
        {
            var result = new List<Domain.Almacen.Almacen>();

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string database = $"B1H_VIST_{location}";
            string query = $"CALL \"{database}\".\"API_WMS_WHS_GetWarehouses\"()";

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new Domain.Almacen.Almacen
                {
                    CompanyId = companyId,
                    DefaultBin = Convert.ToInt32(reader["DefaultBin"]),
                    EnableBinLocations = Convert.ToString(reader["EnableBinLocations"]) ?? string.Empty,
                    Sucursal = Convert.ToString(reader["Sucursal"]) ?? string.Empty,
                    WarehouseCode = Convert.ToString(reader["WarehouseCode"]) ?? string.Empty,
                    WarehouseName = Convert.ToString(reader["WarehouseName"]) ?? string.Empty,
                    WmsLocation = Convert.ToString(reader["WmsLocation"]) ?? string.Empty
                });
            }

            return result;
        }
    }
}
