using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Purchases
    {
        public long Id { set; get; }
        public long Quantity { set; get; }
        [ForeignKey("Transactions")]
        public long Transaction { get; set; }
        public Transactions Transactions { get; set; }
    }
}
