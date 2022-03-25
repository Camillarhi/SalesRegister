using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.DTOs
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public DateTime Expiration { get; set; }
    }
}
