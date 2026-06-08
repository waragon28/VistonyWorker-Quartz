using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Comisiones.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class ComisionesHanaRepository : IComisionesRepository
    {
        private readonly string _connectionString;

        public ComisionesHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task EjecutarAsync(string database)
        {
            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string query =
                $"CALL \"{database}\".\"P_VIS_VEN_COMISIONES_ACTU\"()";

            await using var command = new HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
