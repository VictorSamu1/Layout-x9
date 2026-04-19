using System;
using System.ComponentModel.DataAnnotations;

namespace TesteX9.Models
{
    public class Ocorrencia
    {
        [Key]
        public int Id { get; set; }
        
        public int AlunoId { get; set; }
        public Aluno Aluno { get; set; }
        public int FuncionarioId { get; set; }
        public Funcionario RegistradoPor { get; set; }

        public DateTime DataOcorrencia { get; set; }
        public TimeSpan Horario { get; set; }
        public string TurmaNoMomento { get; set; }
        public string Materia { get; set; }
        public string ProfessorHorario { get; set; }
        
        public string TiposInfracao { get; set; } 
        public string Descricao { get; set; }
        public DateTime DataRegistroSistema { get; set; } = DateTime.Now;
    }
}