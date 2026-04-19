using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TesteX9.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados SQLite
builder.Services.AddDbContext<EscolaContext>(options =>
    options.UseSqlite("Data Source=banco_escola.db"));

builder.Services.AddControllers();

// Adiciona CORS para o seu frontend HTML conseguir aceder
builder.Services.AddCors(options => {
    options.AddPolicy("PermitirTudo", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors("PermitirTudo");
app.MapControllers();
app.MapGet("/", () => "API Rodando!");

app.Run();