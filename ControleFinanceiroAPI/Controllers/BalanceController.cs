using ControleFinanceiroAPI.Data;
using ControleFinanceiroAPI.DTOs.Monthl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ControleFinanceiroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BalanceController : ControllerBase
{
    private readonly AppDbContext _context;

    public BalanceController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetMonthlyBalance([FromQuery] int month, [FromQuery] int year)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if(userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userid))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        if ((month < 1 || month > 12))
            return BadRequest("O Mês deve ser entre 1 e 12");

        if ((year < 2000))
            return BadRequest("Ano Invalido");


        var totalIncomes = await _context.Incomes
            .Where(i => i.UserId == userid && i.Data.Month == month && i.Data.Year == year)
            .SumAsync(i => (decimal?)i.Amount) ?? 0;

        var totalExpenses = await _context.Expenses
            .Where(e => e.UserId == userid && e.Data.Month == month && e.Data.Year == year)
            .SumAsync(e => (decimal?)e.Amount) ?? 0;

        var response = new MonthlyBalanceResponseDto
        {
            Month = month,
            Year = year,
            TotalIncomes = totalIncomes,
            TotalExpense = totalExpenses,
            Balance = totalIncomes - totalExpenses
        };

        return Ok(response);
    }
}
