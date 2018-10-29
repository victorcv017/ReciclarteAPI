
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
        [ForeignKey("Office")]
        public string OfficesId { get; set; }
        public Offices Offices { get; set; }
    }
}
