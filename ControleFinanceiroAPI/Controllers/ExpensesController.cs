using ControleFinanceiroAPI.Data;
using ControleFinanceiroAPI.DTOs.Expenses;
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
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Endpoint para adicionar uma nova despesa ao usuario
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddExpense(CreateExpenseDto dto)
    {
        //Validação automatica para os Data Annotations
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Extrai o userId pelo Jwt
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Validação do token
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Adicionando os dados via dto
        var expense = new Expense
        {
            Amount = dto.Amount,
            Data = dto.Data,
            Description = dto.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        //Persistindo no banco de dados
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        //Retornando os dados desejados via dto
        var response = new ExpenseResponseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Data = expense.Data.ToString("dd/MM/yyyy"),
            Description = expense.Description
        };

        return Created("", response);
    }


    /// <summary>
    /// Retorna a lista de despesas do usuario. Pode ser filtrado por mês e ano
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
       [FromQuery] int? month,
       [FromQuery] int? year)
    {
        //Extrai o userId pelo Jwt
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Valida o token
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Valida se tem o valor e se ele é valido
        if (month.HasValue && (month < 1 || month > 12))
            return BadRequest("O Mês deve ser entre 1 e 12");

        //Valida se tem o valor e se é valido
        if (year.HasValue && (year < 2000))
            return BadRequest("Ano inválido");

        //Cria a querry base para o usuario logado
        IQueryable<Expense> querry = _context.Expenses
            .Where(e => e.UserId == userId);

        //Aplica filtro por ano, se informado
        if (year.HasValue)
        {
            querry = querry.Where(e => e.Data.Year == year.Value);
        }

        //Aplica filtro por mes, se informado
        if (month.HasValue)
        {
            querry = querry.Where(e => e.Data.Month == month.Value);
        }

        //Retorna os dados por ordem decrescente
        var expenses = await querry
            .OrderByDescending(e => e.Data)
            .Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Amount = e.Amount,
                Data = e.Data.ToString("dd/MM/yyyy"),
                Description = e.Description
            }).ToListAsync();

        return Ok(expenses);

    }

    /// <summary>
    /// Atualiza Despesa existente do usuario
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(int id, UpdateExpenseDto dto)
    {

        //Validação automatica para os Data Annotations
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Extrai o userId pelo Jwt
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Validação do token
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Valida a despesa via id da despesa e pelo id do usuario
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.UserId == userId && e.Id == id);

        //Retorno para caso despesa não encontrada
        if (expense == null)
            return NotFound("Despesa não encontrada");

        //Atualização dos dados via dto
        expense.Amount = dto.Amount;
        expense.Data = dto.Data;
        expense.Description = dto.Description;

        //Persistindo no banco
        await _context.SaveChangesAsync();

        //Retorno dos dados atualizados
        var response = new ExpenseResponseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Data = expense.Data.ToString("dd/MM/yyyy"),
            Description = expense.Description
        };

        return Ok(response);
    }

    /// <summary>
    /// Deleta uma despesa existente do usuario
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {

        //Extrai o userId pelo Jwt
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        //Validação do token
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        //Valida a despesa via id da despesa e pelo id do usuario
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        //Retorno para caso despesa não encontrada
        if (expense == null)
            return NotFound("Despesa não encontrada");

        //Deleta a despesa e Persistindo no banco
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();

    }


}
