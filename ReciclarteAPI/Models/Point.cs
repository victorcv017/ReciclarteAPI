using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class Point
    {
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
