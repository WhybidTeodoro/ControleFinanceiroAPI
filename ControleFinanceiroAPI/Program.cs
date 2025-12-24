using ControleFinanceiroAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// ========================
//  Configuração DbContext
// ========================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});

// ====================
//  Configuração JWT
// ====================

//Le a seção Jwt de appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

//obtém a chave secreta e converte para bytes
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

//Registra o serviço de autenticação
builder.Services.AddAuthentication(options =>
{
    //define que o padrão de autenticação sera JWT
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //valida se o token foi assinado com a chave correta
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),

        //valida quem emitiu o token
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],

        //Valida para quem o token é destinado
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],

        //valida se o token expirou
        ValidateLifetime = true,

        //Remove tolerancia de tempo extra
        ClockSkew = TimeSpan.Zero
    };
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
