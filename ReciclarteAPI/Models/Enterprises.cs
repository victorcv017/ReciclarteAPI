using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReciclarteAPI.Models
{
	public class Enterprises : IdentityUser
	{
        public string Name { get; set; }
        [Url]
        public string Logo { get; set; }
        [DefaultValue(0)]
        public double Balance { get; set; }
        public string RFC { get; set; }
        public List<Offices> Offices { get; set; }
    }
}