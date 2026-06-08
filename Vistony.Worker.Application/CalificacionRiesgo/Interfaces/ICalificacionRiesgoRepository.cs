using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CalificacionRiesgo.Interfaces
{
    public interface ICalificacionRiesgoRepository
    {
        Task EjecutarAsync(string database, string procedure);
    }
}
