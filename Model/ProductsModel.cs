﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class ProductsModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductCode { get; set; }
        [Required]

        public string Product { get; set; }

        [Required]
        public string Measure { get; set; }

        //public string BarcodeImage { get; set; }
        [Required]

        public float UnitPrice { get; set; }

       
    }
}
