using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.OrdenFabricacion.Interfaces
{
    public interface IOrdenFabricacionRepository
    {
        Task<List<Domain.OrdenFabricacion.OrdenFabricacion>> ObtenerNuevasAsync(string database);
        Task MarcarMigradasAsync(List<int> docNums, string database);
    }
}
