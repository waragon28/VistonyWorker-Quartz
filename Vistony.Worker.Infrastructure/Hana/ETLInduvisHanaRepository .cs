using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.ETL.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public sealed class ETLInduvisHanaRepository : IETLInduvisRepository
    {
        private readonly string _connectionString;
        public ETLInduvisHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR")!;
        }
        public async Task EjecutarAsync(string database, string procedure)
        {
            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();
            string query = $"CALL \"{database}\".\"{procedure}\"()";
            await using var command = new HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
