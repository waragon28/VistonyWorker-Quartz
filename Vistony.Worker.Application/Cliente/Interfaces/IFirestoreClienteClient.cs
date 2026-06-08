using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Cliente.Interfaces
{
    public interface IFirestoreClienteClient
    {
        Task<bool> ExisteAsync(string codeSap);
        Task GuardarAsync(Domain.Cliente.Cliente cliente);
    }
}
