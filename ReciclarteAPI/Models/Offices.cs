using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Offices
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        public string Point { get; set; }
        [ForeignKey("Enterprise")]
        public string EnterpriseId { get; set; }
        [JsonIgnore]
        public Enterprises Enterprise { get; set; }
        [ForeignKey("Address")]
        public long AddressId { get; set; }
        public Addresses Address { get; set; }
    }
}
