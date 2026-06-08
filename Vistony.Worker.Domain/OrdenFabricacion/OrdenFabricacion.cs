using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.OrdenFabricacion
{
    public class OrdenFabricacion
    {
        public int OT_Mezcla { get; set; }
        public int OT_Envase { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string UomName { get; set; } = string.Empty;
        public double PlannedQty { get; set; }
    }
}
