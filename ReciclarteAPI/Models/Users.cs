using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ReciclarteAPI.Models
{
    public class Users : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Curp { get; set; }
        public string Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Signature { get; set; } 
        [ForeignKey("Address")]
        public long AddressId { get; set; }
        public Addresses Address { get; set; }
        public List<Transactions> Transactions { get; set; }

        /*ForeignKey("Pais")]
        public int PaisId { get; set; }
        [JsonIgnore]
        public Pais Pais { get; set; }
        */
    }
}
