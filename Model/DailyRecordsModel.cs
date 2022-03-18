using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class DailyRecordsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public int Quantity { get; set; }

        
        [Required]
        [ForeignKey(nameof(ProductsModel.Product))]
        public string Product { get; set; }

        [Required]
        [ForeignKey(nameof(ProductsModel.Measure))]
        public string Measure { get; set; }

        [Required]
        [ForeignKey(nameof(ProductsModel.UnitPrice))]
        public float UnitPrice { get; set; }

        [Required]
        public float Amount { get; set; }

        public DateTime Date { get; set; }

        public string CustomerName { get; set; }
    }
}
