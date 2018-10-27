using Microsoft.AspNetCore.Identity;

namespace ReciclarteAPI.Models
{
	public class Enterprises : IdentityUser
	{
        public string Name { get; set; }
        public string Balance { get; set; }
        public string RFC { get; set; }
    }
}