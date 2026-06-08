using Microsoft.Extensions.Configuration;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.CuentaSalesForce.Interfaces;

namespace Vistony.Worker.Infrastructure.Hana
{
    public class CuentaSalesForceHanaRepository : ICuentaSalesForceRepository
    {
        private readonly string _connectionString;

        public CuentaSalesForceHanaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HanaConnectionSEIDOR_PERU")!;
        }

        public async Task<List<Domain.CuentaSalesForce.CuentaSalesForce>> ObtenerPendientesAsync(string database)
        {
            var result = new List<Domain.CuentaSalesForce.CuentaSalesForce>();

            await using var connection = new HanaConnection(_connectionString);
            await connection.OpenAsync();

            string query =
                $"SELECT " +
                $"T0.\"U_U_SF_Cod\", " +
                $"T0.\"GroupNum\", " +
                $"T0.\"CreditLine\" " +
                $"FROM \"{database}\".\"OCRD\" T0 " +
                $"WHERE T0.\"U_SF_StatusMigration\" = 'Y'";

            await using var command = new HanaCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new Domain.CuentaSalesForce.CuentaSalesForce
                {
                    CodigoSalesForce = reader["U_U_SF_Cod"] == DBNull.Value ? string.Empty : Convert.ToString(reader["U_U_SF_Cod"])!,
                    GroupNum = reader["GroupNum"] == DBNull.Value ? 0 : Convert.ToInt32(reader["GroupNum"]),
                    CreditLine = reader["CreditLine"] == DBNull.Value ? 0.0 : Convert.ToDouble(reader["CreditLine"])
                });
            }

            return result;
        }
    }
}
