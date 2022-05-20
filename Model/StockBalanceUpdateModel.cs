﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class StockBalanceUpdateModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string AdminId { get; set; }
        [Required]
        public List<StockBalanceUpdateDetailsModel> stockBalanceUpdateDetails { get; set; }
    }
}