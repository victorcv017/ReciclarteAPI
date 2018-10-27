﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Centers
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Schedule { get; set; }
        [ForeignKey("Address")]
        public long AddressId { get; set; }
        public Addresses Address { get; set; }
        public List<MaterialsPerCenter> MaterialsPerCenters { get; set; }
    }
}
