using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class CustomerInvoiceModelDTO
    {

        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public float Total { get; set; }
        public List<CustomerInvoiceDetailModelDTO> InvoiceDetail { get; set; }
    }
}
