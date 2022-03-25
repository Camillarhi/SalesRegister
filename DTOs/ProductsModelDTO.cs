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
        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        //public IFormFile BarcodeImage { get; set; }

    }
}
