using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Application.CategoriaCliente.UseCases
{
    public class ObtenerCategoriaClienteJob
    {
        private readonly ObtenerCategoriaClienteUseCase _useCase;
        private readonly ILogger<ObtenerCategoriaClienteJob> _logger;

        public ObtenerCategoriaClienteJob(
            ObtenerCategoriaClienteUseCase useCase,
            ILogger<ObtenerCategoriaClienteJob> logger)
        {
            _useCase = useCase;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            _logger.LogInformation("INICIO JOB CATEGORIA_CLIENTE");
            Console.WriteLine("INICIO JOB CATEGORIA_CLIENTE");

            await _useCase.ExecuteAsync();

            Console.WriteLine("FIN JOB CATEGORIA_CLIENTE");
            _logger.LogInformation("FIN JOB CATEGORIA_CLIENTE");
        }
    }
}
