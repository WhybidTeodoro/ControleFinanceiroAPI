using ControleFinanceiroAPI.Data;
using ControleFinanceiroAPI.DTOs.Users;
using ControleFinanceiroAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControleFinanceiroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UsersController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        bool emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            return BadRequest("Email já existe");

        var user = new User
        {
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Created("", new {user.Id, user.Email}); 
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login (LoginUserDto dto)
    {

        //Busca o usuario pelo email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized("Email ou senha invalidos");

        //Verifica senha pelo PasswordHasher
        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Email ou senha invalidos");

        //Cria as claim(dados dos usuarios no token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var jwtsettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtsettings["Key"]!);

        //cria as credenciais de assinatura
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

        //cria o token
        var token = new JwtSecurityToken(
            issuer: jwtsettings["Issuer"],
            audience: jwtsettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                double.Parse(jwtsettings["ExpiresInMinutes"]!)),
                signingCredentials: credentials);


        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

}
