
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Materials
    {
        public long Id { set; get; }
        [JsonProperty(PropertyName = "name")]
        public string Material { set; get; }
        public double Price { set; get; }
        [JsonIgnore]
        public List<MaterialsPerCenter> MaterialsPerCenters { get; set; }
    }
}
