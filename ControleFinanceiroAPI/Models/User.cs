using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroAPI.Models;

/// <summary>
/// Representa o usuario do sistema.
/// </summary>
public class User
{
    /// <summary>
    /// Identificador unico do usuario
    /// </summary>
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Email do usuario. Usado para login
    /// </summary> 
    [Required(ErrorMessage = "O Email é obrigatório")]
    [EmailAddress(ErrorMessage = "O email informado não é valido")]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Hash da senha do usuario. Usado para login
    /// </summary>
    [Required(ErrorMessage = "A Senha é obrigatória")]
    public string PasswordHash { get; set; } = string.Empty;
    /// <summary>
    /// Data da criação do usuario. Automaticamente setado ao criar
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Relacionamento 1:N entre usuario e receitas
    /// </summary>
    public ICollection<Income> Incomes { get; set; } = new List<Income>();

    /// <summary>
    /// Relacionamento 1:N entre usuario e despesas
    /// </summary>
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
