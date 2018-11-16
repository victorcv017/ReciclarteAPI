using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Addresses
    {
        public long Id { get; set; }
        public string City { get; set; }
        public string Township { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public int PC { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
        [JsonIgnore]
        public Users User { get; set; }
        [JsonIgnore]
        public string CenterId { get; set; }
        [JsonIgnore]
        public Centers Center { get; set; }
        [JsonIgnore]
        public string OfficeId { get; set; }
        [JsonIgnore]
        public Offices Office { get; set; }


    }
}
