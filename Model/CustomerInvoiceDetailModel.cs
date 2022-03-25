using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class CustomerInvoiceDetailModel
    {
        [Required]
        public  string Id { get; set; }

        [Required]
        [ForeignKey(nameof(CustomerInvoiceModel.InvoiceId))]
        public string InvoiceId { get; set; }

        public string AdminId { get; set; }

        [Required]
        public string Product { get; set; }
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string Measure { get; set; }

        [Required]
        public string MeasureId { get; set; }

        [Required]
        public int Quantity { get; set; }       

        [Required]
        public float UnitPrice { get; set; }

        public float Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
