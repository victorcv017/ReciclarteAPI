using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReciclarteAPI.Models
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Curp { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public char Gender { get; set; }
        public string Signature { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Addresses Address { get; set; }
        public List<Transactions> Transactions { get; set; }
    }
}
