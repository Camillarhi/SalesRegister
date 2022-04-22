using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class StockInwardModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string AdminId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool Approve { get; set; }
        public List<StockInwardDetailsModel> stockInwardDetails { get; set; }
    }
}
