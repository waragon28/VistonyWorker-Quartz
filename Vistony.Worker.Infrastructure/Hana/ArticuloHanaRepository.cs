using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Articulo.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class ArticuloHanaRepository : IArticuloRepository
    {
        private readonly string _connectionString;

        public ArticuloHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task<List<string>> ObtenerCodigosAsync(string location)
        {
            var result = new List<string>();

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string database = $"B1H_VIST_{location}";
            string query = $"CALL \"{database}\".\"API_WMS_WHS_GETITEM\"()";

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var itemCode = Convert.ToString(reader["ItemCode"]);

                if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    result.Add(itemCode);
                }
            }

            return result;
        }

        public async Task<Domain.Articulo.Articulo?> ObtenerDetalleAsync(
            string location,
            string companyId,
            string itemCode)
        {
            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string database = $"B1H_VIST_{location}";

            string query =
                $"CALL \"{database}\".\"API_WMS_WHS_GETITEM_PARAMETERS\"('{itemCode}')";

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Domain.Articulo.Articulo
            {
                CompanyId = companyId,
                CorpLine = Convert.ToString(reader["U_VIS_Corp_Line"]) ?? string.Empty,
                InventoryWeight = Convert.ToDouble(reader["InventoryWeight"]),
                ItemCode = Convert.ToString(reader["ItemCode"]) ?? string.Empty,
                ItemName = Convert.ToString(reader["ItemName"]) ?? string.Empty,
                ItemsGroupCode = Convert.ToString(reader["ItemsGroupCode"]) ?? string.Empty,
                PurchaseUnitHeight = Convert.ToString(reader["PurchaseUnitHeight"]) ?? string.Empty,
                PurchaseUnitLength = Convert.ToString(reader["PurchaseUnitLength"]) ?? string.Empty,
                PurchaseUnitWidth = Convert.ToString(reader["PurchaseUnitWidth"]) ?? string.Empty,
                QtyPallet = Convert.ToDouble(reader["QtyPallet"]),
                UoMGroupEntry = Convert.ToString(reader["UoMGroupEntry"]) ?? string.Empty
            };
        }
    }
}
