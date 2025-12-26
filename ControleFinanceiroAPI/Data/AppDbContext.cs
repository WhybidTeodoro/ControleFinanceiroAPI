using ControleFinanceiroAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Income> Incomes => Set<Income>();

    public DbSet<Expense> Expenses => Set<Expense>();
}
