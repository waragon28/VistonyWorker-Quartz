using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.CuentaSalesForce
{
    public class CuentaSalesForce
    {
        public string CodigoSalesForce { get; set; } = string.Empty;
        public int GroupNum { get; set; }
        public double CreditLine { get; set; }
    }
}
