using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TesteX9.Data;
using TesteX9.Models;

namespace TesteX9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposInfracaoController : ControllerBase
    {
        private readonly EscolaContext _context;
        public TiposInfracaoController(EscolaContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> Listar() => Ok(await _context.TiposInfracao.ToListAsync());


         // ADICIONA NOVA INFRAÇÃO E LANÇA O ERRO SE CASO ELA JA EXISTIR
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] TipoInfracao novo)
        {
            if (await _context.TiposInfracao.AnyAsync(t => t.Nome == novo.Nome))
                return BadRequest(new { mensagem = "Este tipo já existe!" });

            _context.TiposInfracao.Add(novo);
            await _context.SaveChangesAsync();
            return Ok(novo);
        }
    }
}