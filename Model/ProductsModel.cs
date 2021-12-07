using System;
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
           
        public string Product { get; set; }

        public float UnitPrice { get; set; }

       
    }
}
