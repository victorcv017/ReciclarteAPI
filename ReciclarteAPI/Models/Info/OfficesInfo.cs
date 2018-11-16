using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class OfficesInfo
    {
        public string Id { get; set; }
        public Schedule Schedule { get; set; }
        public Point Point { get; set; }
        public string EnterpriseId { get; set; }
        public Enterprises Enterprise { get; set; }
        public Addresses Address { get; set; }
        public List<Items> Items { get; set; }
    }
}
