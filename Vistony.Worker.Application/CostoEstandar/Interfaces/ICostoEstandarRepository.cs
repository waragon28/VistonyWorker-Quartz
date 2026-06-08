using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CostoEstandar.Interfaces
{
    public interface ICostoEstandarRepository
    {
        Task EjecutarAsync(string database);
    }
}
