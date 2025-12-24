using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiroAPI.Models;

/// <summary>
/// Representa o usuario do sistema.
/// </summary>
public class User
{
    /// <summary>
    /// Identificador unico do usuario
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Email do usuario. Usado para login
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Hash da senha do usuario. Usado para login
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;
    /// <summary>
    /// Data da criação do usuario. Automaticamente setado ao criar
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
