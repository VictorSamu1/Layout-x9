using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteX9.Data;
using TesteX9.Models;

namespace TesteX9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EscolaContext _context;

        public AuthController(EscolaContext context)
        {
            _context = context;
        }

        // CRIA O DIRETOR
        [HttpPost("registrar-teste")]
        public async Task<IActionResult> RegistrarTeste()
        {
            if (await _context.Funcionarios.AnyAsync(f => f.Email == "diretor@escola.com"))
            {
                return BadRequest(new { mensagem = "O usuário diretor já existe no banco!" });
            }

            var diretor = new Funcionario
            {
                Nome = "Diretor Chefe",
                Email = "diretor@escola.com",
                SenhaHash = "123456",
                Cargo = "Direção"
            };

            _context.Funcionarios.Add(diretor);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Usuário teste criado com sucesso!" });
        }

        // CRIA O ALUNO
        [HttpPost("criar-aluno-teste")]
        public async Task<IActionResult> CriarAlunoTeste()
        {
            if (await _context.Alunos.AnyAsync(a => a.Id == 1))
            {
                return BadRequest(new { mensagem = "O aluno teste já existe no banco!" });
            }

            var aluno = new Aluno
            {
                Id = 1,
                NomeCompleto = "João Silva Sauro",
                Simade = "123456789",
                DataNascimento = new System.DateTime(2010, 5, 15),
                Turma = "1º Ano A"
            };

            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Aluno teste criado com sucesso!" });
        }

        // FAZ O LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var funcionario = await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Email == request.Email && f.SenhaHash == request.Senha);

            if (funcionario == null)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            return Ok(new { 
                mensagem = "Login realizado!",
                nome = funcionario.Nome,
                token = "cracha-valido-123" 
            });
        }
    }
}