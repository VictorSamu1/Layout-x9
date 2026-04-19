using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TesteX9.Models
{
    public class Aluno
    {
        [Key]
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Simade { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Turma { get; set; }
        
        // Relacionamento: Um aluno tem várias ocorrências
        public List<Ocorrencia> Ocorrencias { get; set; } 
    }
}