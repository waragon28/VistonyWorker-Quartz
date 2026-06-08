using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vistony.Worker.Domain.Articulo
{
    public class Articulo
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CorpLine { get; set; } = string.Empty;
        public double InventoryWeight { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string ItemsGroupCode { get; set; } = string.Empty;
        public string PurchaseUnitHeight { get; set; } = string.Empty;
        public string PurchaseUnitLength { get; set; } = string.Empty;
        public string PurchaseUnitWidth { get; set; } = string.Empty;
        public double QtyPallet { get; set; }
        public string UoMGroupEntry { get; set; } = string.Empty;
    }
}
