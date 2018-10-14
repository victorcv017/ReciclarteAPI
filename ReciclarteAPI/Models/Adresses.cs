using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Adresses
    {
        public long Id { get; set; }
        public string City { get; set; }
        public string Township { get; set; }
        public string Street { get; set; }
        public long Number { get; set; }
        public long PC { get; set; }
    }
}
