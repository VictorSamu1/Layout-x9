using Microsoft.EntityFrameworkCore;
using TesteX9.Data;

// 1. ESTA É A LINHA QUE TINHA SUMIDO! (Cria o construtor da API)
var builder = WebApplication.CreateBuilder(args);

// 2. CONFIGURAÇÃO DO BANCO DE DADOS (MySQL do XAMPP)
var stringConexao = "Server=localhost;Database=sistema_escola;User=root;Password=;";

builder.Services.AddDbContext<EscolaContext>(options =>
    options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao)));

// 3. Adiciona suporte para os Controllers (As suas rotas)
builder.Services.AddControllers();

// 4. Configuração do CORS (Para o seu index.html conseguir ligar-se à API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        b => b.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// 5. Ativa as configurações
app.UseCors("PermitirTudo");
app.MapControllers();

app.Run();