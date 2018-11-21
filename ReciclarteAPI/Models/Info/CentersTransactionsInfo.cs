using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class CentersTransactionsInfo
    {
        public long Id { get; set; }
        public double Weight { get;set; }
        public double Total { get; set; }
        public DateTime Date { get; set; }
        public Materials Material { get; set; }
        public UsersInfo User { get; set; }
    }
}
