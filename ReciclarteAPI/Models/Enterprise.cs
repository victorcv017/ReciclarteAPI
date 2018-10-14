namespace ReciclarteAPI.Models
{
	public class Enterprise
	{
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } //Agregar Unique
        public string Password { get; set; }
        public string Balance { get; set; }
        public string RFC { get; set; } //Agregr unique
    }
}