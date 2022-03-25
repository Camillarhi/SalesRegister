using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class ProductMeasureDTO
    {
        public string Quantity { get; set; }
        public string Measure { get; set; }
        public string QtyPerMeasure { get; set; }
        public float CostPrice { get; set; }
        public float UnitPrice { get; set; }
    }
}
