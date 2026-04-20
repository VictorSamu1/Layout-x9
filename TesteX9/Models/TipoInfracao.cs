using System.ComponentModel.DataAnnotations;

namespace TesteX9.Models
{
    public class TipoInfracao
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } // Ex: "Uso de Celular", "Bullying"
    }
}