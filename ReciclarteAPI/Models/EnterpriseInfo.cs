using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class EnterpriseInfo
    {
        public string Id { get; set; }
        [JsonIgnore]
        public string RFC { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public List<Offices> Offices { get; set; }
    }
}
