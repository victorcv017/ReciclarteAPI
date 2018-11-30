using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace ReciclarteAPI.Models
{
    public class Users : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        [RegularExpression(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$", ErrorMessage = "Curp Inválido")]
        [StringLength(18, ErrorMessage = "Curp no válido")]
        public string Curp { get; set; }
        [DefaultValue(0)]
        public double Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [DefaultValue('N')]
        public char Gender { get; set; }
        public string Signature { get; set; } 
        public List<Transactions> Transactions { get; set; }
        public Addresses Address { get; set; }
    }
}
