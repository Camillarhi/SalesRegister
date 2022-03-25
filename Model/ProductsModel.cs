using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class ProductsModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string ProductCode { get; set; }
        [Required]

        public string ProductName { get; set; }

        [Required]
        [ForeignKey(nameof(IdentityUser.Id))]
        public string AdminId { get; set; }

        public List<ProductMeasureModel> ProductMeasures { get; set; }

    }
}
