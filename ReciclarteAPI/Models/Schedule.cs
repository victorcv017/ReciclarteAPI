using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Schedule
    {
        public string Lu { get; set; }
        public string Ma { get; set; }
        public string Mi { get; set; }
        public string Ju { get; set; }
        public string Vi { get; set; }
        public string Sa { get; set; }
        public string Do { get; set; }
    }
}
