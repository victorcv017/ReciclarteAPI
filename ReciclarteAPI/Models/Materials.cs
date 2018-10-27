
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Materials
    {
        public long Id { set; get; }
        public string Material { set; get; }
        public long Price { set; get; }
        public List<MaterialsPerCenter> MaterialsPerCenters { get; set; }
    }
}
