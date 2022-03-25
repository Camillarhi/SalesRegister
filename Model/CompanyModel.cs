using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class CompanyModel
    {
        [Required]
        public string Id { get; set; }

        [Required]

        public string CompanyName { get; set; }

        [Required]
        [ForeignKey(nameof(IdentityUser.Id))]
        public string AdminId { get; set; }
    }
}
