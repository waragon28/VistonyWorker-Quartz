using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CategoriaCliente.Interfaces
{
    public interface ICategoriaClienteRepository
    {
        Task EjecutarAsync(string database, string procedure);
    }
}
