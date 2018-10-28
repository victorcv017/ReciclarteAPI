using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ReciclarteAPI.Models
{
	public class Enterprises : IdentityUser
	{
        public string Name { get; set; }
        public string Balance { get; set; }
        public string RFC { get; set; }
        public List<Offices> Offices { get; set; }
    }
}