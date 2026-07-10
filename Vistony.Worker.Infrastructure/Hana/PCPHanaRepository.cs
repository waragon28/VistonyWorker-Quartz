using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PCP.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class PCPHanaRepository : IPCPRepository
    {
        private readonly string _connectionString;
        private readonly string _connectionStringInduvis;

        public PCPHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU");
            _connectionStringInduvis = configuration.GetConnectionString("HanaConnectionSEIDOR");
        }

        public async Task EjecutarBackorder(string database)
        {
            string Conetion = string.Empty;
            if (database == "B1H_VIST_PE")
            {
                Conetion = _connectionString;
            }
            else
            {
                Conetion = _connectionStringInduvis;
            }
            await using var connection = new Sap.Data.Hana.HanaConnection(Conetion);
            await connection.OpenAsync();
            string query =
                $"CALL \"{database}\".\"BACKORDER_PCP\"()";
            await using var command = new Sap.Data.Hana.HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task EjecutarStockDiario(string database)
        {
            string Conetion = string.Empty;
            if (database == "B1H_VIST_PE")
            {
                Conetion = _connectionString;
            }
            else
            {
                Conetion = _connectionStringInduvis;
            }
            await using var connection = new Sap.Data.Hana.HanaConnection(Conetion);
            await connection.OpenAsync();
            string query =
                $"CALL \"{database}\".\"STOCK_DIARIO_PCP\"()";
            await using var command = new Sap.Data.Hana.HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
