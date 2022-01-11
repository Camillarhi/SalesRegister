using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class CustomerInvoiceModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public DateTime Date { get; set; }

        public float Total { get; set; }
    }
}
