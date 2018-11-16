using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class SalesInfo
    {
        public string UserId { get; set; }
        public Dictionary<long,double> Materials { get; set; }
        public long Center { get; set; }
    }
}
