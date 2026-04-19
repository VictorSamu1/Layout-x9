using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TesteX9.Data;
using TesteX9.Models;

namespace TesteX9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OcorrenciasController : ControllerBase
    {
        private readonly EscolaContext _context;

        public OcorrenciasController(EscolaContext context) { _context = context; }

        [HttpPost]
        public async Task<IActionResult> RegistrarOcorrencia([FromBody] Ocorrencia novaOcorrencia)
        {
            _context.Ocorrencias.Add(novaOcorrencia);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Sucesso!" });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarOcorrencias(string turma, int? alunoId, string tipoInfracao)
        {
            var query = _context.Ocorrencias.Include(o => o.Aluno).Include(o => o.RegistradoPor).AsQueryable();
            var resultados = await query.OrderByDescending(o => o.DataOcorrencia).ToListAsync();
            return Ok(resultados);
        }
    }
}