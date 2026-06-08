using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.UpdateWorkPath.Interfaces
{
    public interface IUpdateWorkPathRepository
    {
        Task EjecutarAsync(string database);
    }
}
