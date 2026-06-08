using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Almacen.Interfaces
{
    public interface IAlmacenRepository
    {
        Task<List<Domain.Almacen.Almacen>> ObtenerAlmacenesAsync(string location, string companyId);
    }
}
