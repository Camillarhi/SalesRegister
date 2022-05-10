using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class StockInwardDetailsModelDTO
    {

        [Required]
        public string Product { get; set; }
        [Required]
        public string Measure { get; set; }
        [Required]
        public int Quantity { get; set; }
       
    }
}
