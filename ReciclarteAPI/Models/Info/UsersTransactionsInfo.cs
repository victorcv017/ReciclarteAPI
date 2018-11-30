using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models.Info
{
    public class UsersTransactionsInfo
    {
        public IEnumerable<UserSalesInfo> Sales { get; set; }
        public IEnumerable<UserPurchasesInfo> Purchases { get; set; }
    }

    public class UserSalesInfo
    {
        public long Id { get; set; }
        public double Weight { get; set; }
        public string Center { get; set; }
        public string Material { get; set; }
        public double Coins { get; set; }
        public DateTime Date { get; set; }

    }

    public class UserPurchasesInfo
    {
        public long Id { get; set; }
        public double Quantity { get; set; }
        public string Enterprise { get; set; }
        public string Item { get; set; }
        public double Coins { get; set; }
        public DateTime Date { get; set; }
    }
}
