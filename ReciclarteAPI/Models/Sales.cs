using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Sales
    {
        public long Id { set; get; }
        public long Weight { set; get; }
        [ForeignKey("Transactions")]
        public long Transaction { get; set; }
        public Transactions Transactions { get; set; }
        [ForeignKey("Centers")]
        public long Center { get; set; }
        public Centers Centers { get; set; }
        [ForeignKey("Materials")]
        public long Material { get; set; }
        public Materials Materials { get; set; }
    }
}
