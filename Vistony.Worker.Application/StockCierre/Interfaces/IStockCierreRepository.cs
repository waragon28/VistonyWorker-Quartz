using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.StockCierre.Interfaces
{
    public interface IStockCierreRepository
    {
        Task EjecutarAsync(string database);
    }
}
