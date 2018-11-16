using Newtonsoft.Json;
using ReciclarteAPI.Models.Info;
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
        public string Logo { get; set; }
        public IEnumerable<OfficesInfo> Offices { get; set; }
    }
}
