using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class ProductMeasureModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [ForeignKey(nameof(ProductsModel.Id))]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string Measure { get; set; }

        public int QtyPerMeasure { get; set; }

        //public string BarcodeImage { get; set; }
        public float CostPrice { get; set; }

        public float UnitPrice { get; set; }
    }
}
