using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class StockBalanceUpdateModelDTO
    {
        public string Product { get; set; }

        public string Measure { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }
        public string AdminId { get; set; }

    }
}
