using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class ProductDetailsDTO
    {
        public string ProductCode { get; set; }

        public string Product { get; set; }

        public List<ProductMeasureDTO> ProductMeasure { get; set; }
    }
}
