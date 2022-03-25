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
        public int InvoiceId { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductCode { get; set; }

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
