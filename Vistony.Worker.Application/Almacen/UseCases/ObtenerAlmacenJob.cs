using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vistony.Worker.Application.OrdenFabricacion.UseCases;

namespace Vistony.Worker.Application.Almacen.UseCases
{
    public class ObtenerAlmacenJob
    {
        private readonly ObtenerAlmacenUseCase _useCase;
        private readonly ILogger<ObtenerAlmacenJob> _logger;

        public ObtenerAlmacenJob(
            ObtenerAlmacenUseCase useCase,
            ILogger<ObtenerAlmacenJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ALMACEN");
            Console.WriteLine("INICIO JOB ALMACEN");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB ALMACEN");
            _logger.LogInformation("FIN JOB ALMACEN");
        }
    }
}
