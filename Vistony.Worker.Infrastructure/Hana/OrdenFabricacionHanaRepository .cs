using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.OrdenFabricacion.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class OrdenFabricacionHanaRepository : IOrdenFabricacionRepository
    {
        private readonly string _connectionString;

        public OrdenFabricacionHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task<List<Domain.OrdenFabricacion.OrdenFabricacion>> ObtenerNuevasAsync(string database)
        {
            var result = new List<Domain.OrdenFabricacion.OrdenFabricacion>();

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string query = $"CALL \"{database}\".\"P_VIS_MIGRACION_OF\"()";
            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new Domain.OrdenFabricacion.OrdenFabricacion
                {
                    OT_Mezcla = reader["DocNumMZ"].ToString()!.ToUpper(),
                    OT_Envase = reader["DocNumEN"].ToString()!.ToUpper(),
                    ItemName = reader["ItemName"].ToString()!.ToUpper(),
                    UomName = reader["UomName"].ToString()!.ToUpper(),
                    PlannedQty = Convert.ToDouble(reader["PlannedQty"])
                });
            }

            return result;
        }

        public async Task MarcarMigradasAsync(List<string> docNums, string database)
        {
            string inClause = string.Join(",", docNums);
            string sql = $"UPDATE \"{database}\".\"OWOR\" " +
                         $"SET \"U_VIS_Alert1\" = 'Y' " +
                         $"WHERE \"DocNum\" IN ({inClause})";

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new HanaCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
