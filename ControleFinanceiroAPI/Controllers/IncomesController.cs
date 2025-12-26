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

        //Verificação de seguraça para o token valido
        if(userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");    
        
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
            Data = income.Data.ToString("dd/mm/yyyy"),
            Description = income.Description
        };


        return Created("", response);
    }

    /// <summary>
    /// Retorna a lista de rendas do usuario. Pode ser filtrado por mês e ano
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? month, 
        [FromQuery] int? year)
    {
     
        //Validações de usuario com JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");
        //Validação basica dos filtros [FromQuerry] recebidos

        if (month.HasValue && (month < 1 || month > 12))
            return BadRequest("O Mês deve ser entre 1 e 12");

        if (year.HasValue && (year < 2000))
            return BadRequest("Ano Inválido");

        //Cria a querry base: apenas rendas de usuarios logados
        IQueryable<Income> querry = _context.Incomes
            .Where(i => i.UserId == userId);

        //Aplica filtro por ano, se informado
        if (year.HasValue)
        {
            querry = querry.Where(i => i.Data.Year == year.Value);
        }

        //Aplica filtro por mes, se informado
        if(month.HasValue)
        {
            querry = querry.Where(i => i.Data.Month == month.Value);
        }


        //executa a querry e mapeia para o dto de resposta
        var incomes = await querry
            .OrderByDescending(i => i.Data)
            .Select(income => new IncomeResponseDto
            {
                Id = income.Id,
                Amount = income.Amount,
                Data = income.Data.ToString("dd/MM/yyyy"),
                Description = income.Description
            }).ToListAsync();

        return Ok(incomes);
    }

    /// <summary>
    /// Atualiza Despesa existente do usuario
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(int id, UpdateIncomeDto dto)
    {
        //Validação automática dos Data Annotations do DTO
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Validações de usuario com JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Verificação de seguraça para o token valido
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Valida a renda via id da despesa e pelo id do usuario
        var income = await _context.Incomes.FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id);
        
        //Retorno para caso renda não encontrada
        if (income == null)
            return NotFound("Renda não encontrada");
        
        //Atualiza os dados via dto
        income.Amount = dto.Amount;
        income.Data = dto.Data;
        income.Description = dto.Description;

        //Persistindo no banco de dados
        await _context.SaveChangesAsync();

        //Retorno os dados atualizados
        var response = new IncomeResponseDto
        {
            Id = id,
            Amount = income.Amount,
            Data = income.Data.ToString("dd/MM/yyyy"),
            Description = income.Description
        };

        return Ok(response);
    }

    /// <summary>
    /// Deleta uma despesa existente do usuario
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        //Validações de usuario com JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Verificação de seguraça para o token valido
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Valida a renda via id da despesa e pelo id do usuario
        var income = await _context.Incomes.FirstOrDefaultAsync(i => i.UserId == userId && i.Id == id);

        //Retorno para caso renda não encontrada
        if (income == null)
            return NotFound("Renda não encontrada");

        //Remove a renda e persiste no banco de dados
        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
