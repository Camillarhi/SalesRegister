using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class ProductsModelDTO
    {
        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Product { get; set; }
        //public IFormFile BarcodeImage { get; set; }
        [Required]
        public string Measure { get; set; }


        [Required]
        public float UnitPrice { get; set; }

    }
}
