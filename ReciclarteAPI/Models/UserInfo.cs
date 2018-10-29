using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Curp { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public Addresses Address { get; set; }
    }
}
