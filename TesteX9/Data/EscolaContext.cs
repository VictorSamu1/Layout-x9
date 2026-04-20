using Microsoft.EntityFrameworkCore;
using TesteX9.Models;

namespace TesteX9.Data
{
    public class EscolaContext : DbContext
    {
        public EscolaContext(DbContextOptions<EscolaContext> options) : base(options) { }

        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<TipoInfracao> TiposInfracao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().HasIndex(a => a.Simade).IsUnique();
        }
    }
}