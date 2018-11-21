using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class OfficesTransactionsInfo
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public Items Item { get; set; }
        public UsersInfo User { get; set; }
    }
}
