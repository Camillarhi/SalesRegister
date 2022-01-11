using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class CustomerInvoiceDetailModelDTO
    {
        [Required]
        [ForeignKey(nameof(CustomerInvoiceModel.Id))]
        public int InvoiceId { get; set; }

        [Required]
        [ForeignKey(nameof(DailyRecordsModel.Product))]
        public string Product { get; set; }

        [Required]
        [ForeignKey(nameof(DailyRecordsModel.Measure))]
        public string Measure { get; set; }

        [Required]
        [ForeignKey(nameof(DailyRecordsModel.Quantity))]

        public int Quantity { get; set; }

        [Required]
        [ForeignKey(nameof(DailyRecordsModel.UnitPrice))]
        public float UnitPrice { get; set; }

        [Required]
        [ForeignKey(nameof(DailyRecordsModel.Amount))]

        public float Amount { get; set; }
    }
}
