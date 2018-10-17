
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Items
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public long Value { set; get; }
        [ForeignKey("Enterprise")]
        public long EnterpriseId { get; set; }
        public Enterprises Enterprise { get; set; }
    }
}
