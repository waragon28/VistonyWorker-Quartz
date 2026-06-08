using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Comisiones.Interfaces
{
    public interface IComisionesRepository
    {
        Task EjecutarAsync(string database);
    }
}
