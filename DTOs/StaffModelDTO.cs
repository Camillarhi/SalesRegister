using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class StaffModelDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        public string Department { get; set; }

        [Required]
        public IFormFile ProfilePicture { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
