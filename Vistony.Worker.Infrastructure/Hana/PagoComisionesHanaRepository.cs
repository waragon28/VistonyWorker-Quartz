using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.PagoComisiones.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class PagoComisionesHanaRepository : IPagoComisionesRepository
    {
        private readonly string _connectionString;

        public PagoComisionesHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task EjecutarAsync(string database)
        {
            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string query =
                $"CALL \"{database}\".\"P_VIS_VEN_PAGO_COMISIONES_ACTU\"()";

            await using var command = new HanaCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
