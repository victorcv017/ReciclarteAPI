using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class EnterprisesTransactionsInfo
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public long TransactionId { get; set; }
        public DateTime Date { get; set; }
        public Items Item { get; set; }
    }
}
