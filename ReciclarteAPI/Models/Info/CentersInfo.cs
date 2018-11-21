using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class CentersInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Schedule Schedule { get; set; }
        public Point Point { get; set; }
        public string Logo { get; set; }
        public Addresses Address { get; set; }
        [JsonProperty(PropertyName = "materials")]
        public List<MaterialsPerCenter> MaterialsPerCenters { get; set; }
    }
}
