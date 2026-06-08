using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.ETLCreditosCobranza.Interfaces
{
    public interface IETLCreditosCobranzaRepository
    {
        Task EjecutarAsync(string database, string procedure);
    }
}
