﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class MaterialsPerCenter
    {
        [JsonIgnore]
        public long MaterialId { get; set; }
        public Materials Material { get; set; }
        [JsonIgnore]
        public string CenterId { get; set; }
        [JsonIgnore]
        public Centers Center { get; set; }
    }
}
