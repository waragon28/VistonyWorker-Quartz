using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.TipoCambio
{
    public sealed class TipoCambioSunat
    {
        public string Origen { get; set; } = string.Empty;
        public decimal Compra { get; set; }
        public decimal Venta { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
