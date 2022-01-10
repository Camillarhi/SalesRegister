using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Model
{
    public class StaffModel:IdentityUser
    {
       
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        
        public string Gender { get; set; }

        
        public DateTime? DateOfBirth { get; set; }

        
        public string Address { get; set; }

       
        public string ProfilePicture { get; set; }

        public string StaffId { get; set; }

    }
}
