using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Offices : IdentityUser
    {
        public Schedule Schedule { get; set; }
        public Point Point { get; set; }
        [ForeignKey("Enterprise")]
        public string EnterpriseId { get; set; }
        [JsonIgnore]
        public Enterprises Enterprise { get; set; }
        public Addresses Address { get; set; }
        public List<Items> Items { get; set; }
    }
}
