using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Cliente.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class ClienteHanaRepository : IClienteRepository
    {
        private readonly string _connectionString;

        public ClienteHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task<List<Domain.Cliente.Cliente>> ObtenerNuevosAsync(string database)
        {
            var result = new List<Domain.Cliente.Cliente>();

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"P_VIS_MIGRACION_CUSTOMERS\"()";

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                DateTime created = DateTime.TryParse(
                    Convert.ToString(reader["Created"]),
                    out DateTime date)
                    ? date.ToUniversalTime()
                    : DateTime.UtcNow;

                result.Add(new Domain.Cliente.Cliente
                {
                    CodeSap = Convert.ToString(reader["CodeSap"])?.Trim() ?? string.Empty,
                    CardCode = Convert.ToString(reader["CardCode"])?.Trim() ?? string.Empty,
                    IdentificationCode = Convert.ToString(reader["IdentificationCode"])?.Trim() ?? string.Empty,
                    Name = Convert.ToString(reader["Name"])?.Trim() ?? string.Empty,
                    Street = Convert.ToString(reader["Street"])?.Trim() ?? string.Empty,
                    Block = Convert.ToString(reader["Block"])?.Trim() ?? string.Empty,
                    Department = Convert.ToString(reader["Department"])?.Trim() ?? string.Empty,
                    Latitude = double.TryParse(Convert.ToString(reader["Latitude"]), out double lat) ? lat : 0.0,
                    Longitude = double.TryParse(Convert.ToString(reader["Longitude"]), out double lng) ? lng : 0.0,
                    Country = Convert.ToString(reader["Country"])?.Trim() ?? string.Empty,
                    Created = Timestamp.FromDateTime(created)
                });
            }

            return result;
        }

        public async Task MarcarMigradosAsync(List<string> cardCodes, string database)
        {
            string inClause = string.Join(",", cardCodes.Select(x => $"'{x}'"));

            string sql = $"UPDATE \"{database}\".\"OCRD\" " +
                         $"SET \"U_SYP_CDRCLI\" = 'Y' " +
                         $"WHERE \"CardCode\" IN ({inClause})";

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new HanaCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
