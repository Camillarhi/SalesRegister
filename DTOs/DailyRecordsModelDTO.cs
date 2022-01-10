using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class DailyRecordsModelDTO
    {
        [Required]
        public int Quantity { get; set; }

      
        [Required]
        [ForeignKey(nameof(ProductsModel.Product))]
        public string Product { get; set; }

        [Required]
        [ForeignKey(nameof(ProductsModel.Product))]
        public string Measure { get; set; }


        [Required]
        [ForeignKey(nameof(ProductsModel.UnitPrice))]
        public float UnitPrice { get; set; }

        public float Amount { get; set; }
    }
}
