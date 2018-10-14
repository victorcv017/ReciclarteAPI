using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReciclarteAPI.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Curp { get; set; } //Agregar Unique
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; }
        public string Signature { get; set; } 
        [ForeignKey("Adresses")]
        public long Adress { get; set; }
        public Adresses Addresses { get; set; }

        /*ForeignKey("Pais")]
        public int PaisId { get; set; }
        [JsonIgnore]
        public Pais Pais { get; set; }
        */
    }
}
