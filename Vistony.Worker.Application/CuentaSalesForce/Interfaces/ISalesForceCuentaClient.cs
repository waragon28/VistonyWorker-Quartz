using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CuentaSalesForce.Interfaces
{
    public interface ISalesForceCuentaClient
    {
        Task ActualizarCuentaAsync(Domain.CuentaSalesForce.CuentaSalesForce cuenta);
    }
}
