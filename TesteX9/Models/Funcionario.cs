using System.ComponentModel.DataAnnotations;

namespace TesteX9.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; } 
        public string SenhaHash { get; set; } 
        public string Cargo { get; set; } 
    }
}