using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class BuyInfo
    {
        public string UserId { get; set; }
        public Dictionary<long, double> Items { get; set; }
    }
}
