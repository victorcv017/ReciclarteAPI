using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace ReciclarteAPI.Models
{
	public class Enterprises : IdentityUser
	{
        public string Name { get; set; }
        public string Logo { get; set; }
        [DefaultValue(0)]
        public string Balance { get; set; }
        public string RFC { get; set; }
        public List<Offices> Offices { get; set; }
    }
}