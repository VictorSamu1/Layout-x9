using Microsoft.EntityFrameworkCore;
using TesteX9.Data;


var builder = WebApplication.CreateBuilder(args);


var stringConexao = "Server=localhost;Database=sistema_escola;User=root;Password=;";

builder.Services.AddDbContext<EscolaContext>(options =>
    options.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao)));


builder.Services.AddControllers();


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

app.Run("http://localhost:5200");