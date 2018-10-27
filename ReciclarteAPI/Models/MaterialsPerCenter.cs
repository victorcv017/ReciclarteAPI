using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class MaterialsPerCenter
    {
        public long MaterialId { get; set; }
        public Materials Material { get; set; }
        public long CenterId { get; set; }
        public Centers Center { get; set; }
    }
}
