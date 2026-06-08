using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Domain.TipoCambio;

namespace Vistony.Worker.Application.TipoCambio.Interfaces
{
    public interface ISunatTipoCambioClient
    {
        Task<TipoCambioSunat?> ObtenerAsync(DateTime fecha);
    }
}
