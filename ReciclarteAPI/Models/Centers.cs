using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Centers
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string schedule { get; set; }
        [ForeignKey("Adresses")]
        public long Adress { get; set; }
        public Adresses Addresses { get; set; }
    }
}
