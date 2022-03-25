using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class AdminFormDTO
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string UserName { get; set; }

        [Required]
        public string Gender { get; set; }
        [Required]
        public string Department { get; set; }

        public DateTime DateOfBirth { get; set; }
       
        [Required]
        public string Address { get; set; }

        public IFormFile ProfilePicture { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
