using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Almacen.Interfaces;

namespace Vistony.Worker.Infrastructure.Firestore
{
    public class FirestoreAlmacenClient : IFirestoreAlmacenClient
    {
        private readonly FirestoreDb _db;

        public FirestoreAlmacenClient(IConfiguration configuration)
        {
            var projectId = configuration["Firestore:ProjectId"]!;
            var keyPath = configuration["Firestore:KeyPath"]!;

            GoogleCredential credential = GoogleCredential.FromFile(keyPath);

            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
        }

        public async Task<string?> ObtenerCompanyIdAsync(string location)
        {
            Query query = _db.Collection("companies")
                .WhereEqualTo("code", location);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Documents.Count == 0)
                return null;

            return snapshot.Documents.First().Id;
        }

        public async Task<bool> ExisteAsync(string companyId, string warehouseCode)
        {
            Query query = _db.Collection("warehouses")
                .WhereEqualTo("companyId", companyId)
                .WhereEqualTo("warehouseCode", warehouseCode);

            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.Count > 0;
        }

        public async Task GuardarAsync(Domain.Almacen.Almacen almacen)
        {
            var item = new
            {
                companyId = almacen.CompanyId,
                defaultBin = almacen.DefaultBin,
                enableBinLocations = almacen.EnableBinLocations,
                sucursal = almacen.Sucursal,
                warehouseCode = almacen.WarehouseCode,
                warehouseName = almacen.WarehouseName,
                wmsLocation = almacen.WmsLocation
            };

            await _db.Collection("warehouses").AddAsync(item);
        }
    }
}
