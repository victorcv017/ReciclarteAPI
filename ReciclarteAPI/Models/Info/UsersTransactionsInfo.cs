using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class UsersTransactionsInfo
    {
        public List<Sales> Sales { get; set; }
        public List<Purchases> Purchases { get; set; }
    }
}
