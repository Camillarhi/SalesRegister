using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class RolesModelDTO
    {
        [Required]

        public string Department { get; set; }

    }
}
