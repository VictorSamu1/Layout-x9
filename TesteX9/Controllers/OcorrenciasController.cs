using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TesteX9.Data;   // Apontando para o local correto agora
using TesteX9.Models; // Apontando para o local correto agora
using Microsoft.AspNetCore.Authorization;

namespace TesteX9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcorrenciasController : ControllerBase
    {
        private readonly EscolaContext _context;

        public OcorrenciasController(EscolaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarOcorrencia([FromBody] Ocorrencia novaOcorrencia)
        {
            if (novaOcorrencia == null) return BadRequest("Dados inválidos");

            _context.Ocorrencias.Add(novaOcorrencia);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Ocorrência registrada com sucesso!", id = novaOcorrencia.Id });
        }
        [Authorize(Roles = "Direção, Supervisão")]
        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarOcorrencias(string turma, int? alunoId, string tipoInfracao)
        {

            var query = _context.Ocorrencias
                .Include(o => o.Aluno)
                .Include(o => o.RegistradoPor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(turma))
                query = query.Where(o => o.Aluno.Turma == turma);

            if (alunoId.HasValue)
                query = query.Where(o => o.AlunoId == alunoId.Value);

            if (!string.IsNullOrEmpty(tipoInfracao))
                query = query.Where(o => o.TiposInfracao.Contains(tipoInfracao));

            var resultados = await query.OrderByDescending(o => o.DataOcorrencia).ToListAsync();

            return Ok(resultados);
        }

        [HttpGet("/api/Alunos/turma/{turma}")]
        public async Task<IActionResult> GetAlunosPorTurma(string turma)
        {
            var alunos = await _context.Alunos
                .Where(a => a.Turma == turma)
                .OrderBy(a => a.NomeCompleto)
                .Select(a => new { a.Id, a.NomeCompleto, a.Simade }) 
                .ToListAsync();

            return Ok(alunos);
        }
    }
}