using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class MaterialPerCenter
    {
        [ForeignKey("Materials")]
        public long Material { get; set; }
        public Materials Materials { get; set; }
        [ForeignKey("Centers")]
        public long Center { get; set; }
        public Centers Centers { get; set; }
    }
}
