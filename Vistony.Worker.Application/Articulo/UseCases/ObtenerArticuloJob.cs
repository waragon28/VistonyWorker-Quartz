using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.Articulo.UseCases
{
    public class ObtenerArticuloJob
    {
        private readonly ObtenerArticuloUseCase _useCase;
        private readonly ILogger<ObtenerArticuloJob> _logger;

        public ObtenerArticuloJob(
            ObtenerArticuloUseCase useCase,
            ILogger<ObtenerArticuloJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB ARTICULO");

            await _useCase.ExecuteAsync();

            _logger.LogInformation("FIN JOB ARTICULO");
        }
    }
}
