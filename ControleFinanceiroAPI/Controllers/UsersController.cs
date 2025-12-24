using ControleFinanceiroAPI.Data;
using ControleFinanceiroAPI.DTOs;
using ControleFinanceiroAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
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
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized("Email ou senha invalidos");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Email ou senha invalidos");

        return Ok(new {user.Id, user.Email});
    }

}
