using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.OrdenFabricacion.Interfaces;

namespace Vistony.Worker.Infrastructure.Firestore
{
    public class FirestoreOrdenFabricacionClient : IFirestoreOrdenFabricacionClient
    {
        private readonly FirestoreDb _db;

        public FirestoreOrdenFabricacionClient(IConfiguration configuration)
        {
            string keyPath = configuration["Firestore:KeyPath"]!;
            string projectId = configuration["Firestore:ProjectId"]!;

            GoogleCredential credential = GoogleCredential.FromFile(keyPath);
            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
        }

        public async Task GuardarAsync(Domain.OrdenFabricacion.OrdenFabricacion orden)
        {
            string docId = orden.OT_Envase.ToString();
            DocumentReference docRef = _db.Collection("ordenes_envase").Document(docId);

            // Respeta la lógica original: skip si ya existe
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists) return;

            var data = new Dictionary<string, object>
            {
                ["OT_Mezcla"] = orden.OT_Mezcla,
                ["OT_Envase"] = orden.OT_Envase,
                ["ItemName"] = orden.ItemName,
                ["UomName"] = orden.UomName,
                ["PlannedQty"] = orden.PlannedQty
            };

            await docRef.SetAsync(data);
        }
    }
}
