using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class UsersProfileInfo
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public Addresses Address { get; set; }
    }
}
