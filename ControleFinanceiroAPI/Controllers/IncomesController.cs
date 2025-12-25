using ControleFinanceiroAPI.Data;
using ControleFinanceiroAPI.DTOs.Incomes;
using ControleFinanceiroAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ControleFinanceiroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class IncomesController : ControllerBase
{
    private readonly AppDbContext _context;

    public IncomesController(AppDbContext context)
    {
        _context = context;
    }


    /// <summary>
    /// Endpoint para Adicionar uma renda ao usuario
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddIncome(CreateIncomeDto dto)
    {
        //Validação automática dos Data Annotations do DTO
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        //Extrai o UserId do JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Verificação de seguração para o token valido
        if(userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");    
        
        //Verificação de seguração para o usuario valido
        bool userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            return Unauthorized("Usuario não encontrado");

        //Adicionando os dados via Dto
        var income = new Income()
        {
            Amount = dto.Amount,
            Data = dto.Data,
            Description = dto.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        //Persistindo os dados no banco
        _context.Incomes.Add(income);
        await _context.SaveChangesAsync();

        //Dto para retornar os dados desejados
        var response = new IncomeResponseDto
        {
            Id = income.Id,
            Amount = income.Amount,
            Data = income.Data,
            Description = income.Description
        };


        return Created("", response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        bool userExists = await _context.Incomes.AnyAsync(u => u.UserId == userId);
        if (!userExists) 
            return Unauthorized("Usuario não encontrado");

        var incomes = await _context.Incomes
            .Where(i => i.UserId == userId)
            .Select(income => new IncomeResponseDto
            {
                Id = income.Id,
                Amount = income.Amount,
                Data = income.Data,
                Description = income.Description
            }).ToListAsync();

        return Ok(incomes);
    }
}
