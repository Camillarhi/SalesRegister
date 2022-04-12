using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class ProductMeasureDTO
    {
        public string Quantity { get; set; }

        [Required]
        public string Measure { get; set; }

        public string QtyPerMeasure { get; set; }

        //public string BarcodeImage { get; set; }
        public float CostPrice { get; set; }

        public float UnitPrice { get; set; }
    }
}
