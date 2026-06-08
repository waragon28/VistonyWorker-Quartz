using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CuentaSalesForce.Interfaces
{
    public interface ICuentaSalesForceRepository
    {
        Task<List<Domain.CuentaSalesForce.CuentaSalesForce>> ObtenerPendientesAsync(string database);
    }
}
