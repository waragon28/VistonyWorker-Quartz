using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.Almacen
{
    public class Almacen
    {
        public string CompanyId { get; set; } = string.Empty;
        public int DefaultBin { get; set; }
        public string EnableBinLocations { get; set; } = string.Empty;
        public string Sucursal { get; set; } = string.Empty;
        public string WarehouseCode { get; set; } = string.Empty;
        public string WarehouseName { get; set; } = string.Empty;
        public string WmsLocation { get; set; } = string.Empty;
    }
}
