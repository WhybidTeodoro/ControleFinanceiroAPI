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

    [HttpPost]
    public async Task<IActionResult> AddExpense(CreateExpenseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized("Token invalido ou sem identificação do usuario");

        var expense = new Expense
        {
            Amount = dto.Amount,
            Data = dto.Data,
            Description = dto.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        var response = new ExpenseResponseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Data = expense.Data.ToString("dd/MM/yyyy"),
            Description = expense.Description
        };

        return Created("", response);
    }
}
