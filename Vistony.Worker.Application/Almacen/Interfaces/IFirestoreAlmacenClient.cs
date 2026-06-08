using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Almacen.Interfaces
{
    public interface IFirestoreAlmacenClient
    {
        Task<string?> ObtenerCompanyIdAsync(string location);
        Task<bool> ExisteAsync(string companyId, string warehouseCode);
        Task GuardarAsync(Domain.Almacen.Almacen almacen);
    }
}
