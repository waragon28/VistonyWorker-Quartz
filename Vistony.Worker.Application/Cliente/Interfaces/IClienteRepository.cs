using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Cliente.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Domain.Cliente.Cliente>> ObtenerNuevosAsync(string database);
        Task MarcarMigradosAsync(List<string> cardCodes, string database);
    }
}
