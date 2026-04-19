using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TesteX9.Data;
using TesteX9.Models;

namespace TesteX9.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EscolaContext _context;

        // Chave secreta para criptografar o token (Em um sistema real, guarde isso em um arquivo .env)
        private const string ChaveSecreta = "MinhaChaveSuperSecretaEscolaX9MuitoLonga123!";

        public AuthController(EscolaContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Busca o funcionário no banco pelo e-mail
            var funcionario = _context.Funcionarios.FirstOrDefault(f => f.Email == request.Email);

            // 2. Verifica se achou e se a senha confere
            // ATENÇÃO: Em produção, nunca compare senhas assim. Use BCrypt ou Argon2 para comparar o Hash!
            if (funcionario == null || funcionario.SenhaHash != request.Senha)
            {
                return Unauthorized(new { mensagem = "E-mail ou senha inválidos." });
            }

            // 3. Preparando o Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(ChaveSecreta);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Guarda informações não-sensíveis DENTRO do token
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, funcionario.Id.ToString()),
                    new Claim(ClaimTypes.Name, funcionario.Nome),
                    new Claim(ClaimTypes.Email, funcionario.Email),
                    new Claim(ClaimTypes.Role, funcionario.Cargo) // O Cargo vai definir o que ele pode acessar!
                }),
                Expires = DateTime.UtcNow.AddHours(4), // Token expira em 4 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // 4. Gera e converte o token para string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 5. Devolve para o Frontend
            return Ok(new { 
                mensagem = "Login bem-sucedido!", 
                token = tokenString,
                cargo = funcionario.Cargo,
                nome = funcionario.Nome
            });
        }
    }
}