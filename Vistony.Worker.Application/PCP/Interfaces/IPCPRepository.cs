using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.PCP.Interfaces
{
    public interface IPCPRepository
    {
        Task EjecutarBackorder(string database);
        Task EjecutarStockDiario (string database);
    }
}
