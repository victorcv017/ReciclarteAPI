using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Transactions
    {
        public long Id { set; get; }
        public double Amount { set; get; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [JsonIgnore]
        public Users User { get; set; }
        public List<Purchases> Purchases { get; set; }
        public List<Sales> Sales { get; set; }
    }
}
