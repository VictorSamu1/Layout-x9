using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TesteX9.Data;

var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DO BANCO DE DADOS ---
builder.Services.AddDbContext<EscolaContext>(options =>
    options.UseSqlite("Data Source=banco_escola.db"));

builder.Services.AddControllers();

// --- CONFIGURAÇÃO DO JWT ---
var chaveSecreta = Encoding.ASCII.GetBytes("MinhaChaveSuperSecretaEscolaX9MuitoLonga123!");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Mude para true se for publicar na internet com HTTPS
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveSecreta),
        ValidateIssuer = false, // Ignora de onde veio por enquanto
        ValidateAudience = false // Ignora pra quem vai por enquanto
    };
});

builder.Services.AddAuthorization(); // Adiciona o serviço de checagem de regras/cargos

var app = builder.Build();

app.UseRouting();

// A ordem importa muito aqui: Primeiro autentica (quem é você?), depois autoriza (o que você pode fazer?)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API do Sistema Escolar está rodando e segura!");

app.Run();