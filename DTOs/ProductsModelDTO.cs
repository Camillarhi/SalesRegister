using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class ProductsModelDTO
    {
        [Required]
        public string ProductName { get; set; }


        public List<ProductMeasureDTO> ProductMeasures { get; set; }

    }
}
