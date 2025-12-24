using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}
