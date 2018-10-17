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
        [ForeignKey("Transaction")]
        public long TransactionId { get; set; }
        public Transactions Transaction { get; set; }
        [ForeignKey("Item")]
        public long ItemId { get; set; }
        public Items Item { get; set; }
    }
}
