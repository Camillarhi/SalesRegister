using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class RolesModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Department { get; set; }

    }
}
