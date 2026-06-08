using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Grpc.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.Cliente.Interfaces;

namespace Vistony.Worker.Infrastructure.Firestore
{
    public class FirestoreClienteClient : IFirestoreClienteClient
    {
        private readonly FirestoreDb _db;

        public FirestoreClienteClient(IConfiguration configuration)
        {
            var projectId = configuration["FirestoreCliente:ProjectId"]!;
            var keyPath = configuration["FirestoreCliente:KeyPath"]!;

            GoogleCredential credential = GoogleCredential.FromFile(keyPath);

            _db = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                ChannelCredentials = credential.ToChannelCredentials()
            }.Build();
        }

        public async Task<bool> ExisteAsync(string codeSap)
        {
            DocumentReference docRef = _db.Collection("customers").Document(codeSap);
            DocumentSnapshot docSnapshot = await docRef.GetSnapshotAsync();

            return docSnapshot.Exists;
        }

        public async Task GuardarAsync(Domain.Cliente.Cliente cliente)
        {
            var item = new
            {
                codeSap = cliente.CodeSap,
                identificationCode = cliente.IdentificationCode,
                name = cliente.Name,
                street = cliente.Street,
                block = cliente.Block,
                department = cliente.Department,
                latitude = cliente.Latitude,
                longitude = cliente.Longitude,
                country = cliente.Country,
                created = cliente.Created
            };

            await _db.Collection("customers")
                .Document(cliente.CodeSap)
                .SetAsync(item);
        }
    }
}
