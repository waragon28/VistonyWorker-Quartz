using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.ETL.Interfaces
{
    public interface IETLInduvisRepository
    {
        Task EjecutarAsync(string database, string procedure);
    }
}
