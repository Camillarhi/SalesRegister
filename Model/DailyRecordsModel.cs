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

        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductsModel.Product))]
        public string Product { get; set; }

        [ForeignKey(nameof(ProductsModel.UnitPrice))]
        public float UnitPrice { get; set; }

        public float Amount { get; set; }
    }
}
