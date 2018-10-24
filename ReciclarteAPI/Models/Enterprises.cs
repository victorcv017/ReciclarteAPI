using Microsoft.AspNetCore.Identity;

namespace ReciclarteAPI.Models
{
	public class Enterprises 
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Balance { get; set; }
        public string Email { get; set; }
        public string Password  { get; set; }
        public string RFC { get; set; } 
    }
}