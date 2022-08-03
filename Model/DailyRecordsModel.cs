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
        public string Product { get; set; }

        [Required]
        public string Measure { get; set; }

        [Required]
        public float UnitPrice { get; set; }

        [Required]
        public float Amount { get; set; }

        public DateTime Date { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string AdminId { get; set; }
        [Required]
        public string SoldById { get; set; }
    }
}
