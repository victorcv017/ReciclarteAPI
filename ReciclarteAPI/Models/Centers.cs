using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Centers : IdentityUser
    {
        public string Name { get; set; }
        public Schedule Schedule { get; set; }
        public Point Point { get; set; }
        [Url]
        public string Logo { get; set; }
        public Addresses Address { get; set; }
        [JsonProperty(PropertyName = "materials")]
        public List<MaterialsPerCenter> MaterialsPerCenters { get; set; }
    }
}
