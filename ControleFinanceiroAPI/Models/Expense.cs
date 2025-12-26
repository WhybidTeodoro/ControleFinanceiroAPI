using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiroAPI.Models;

/// <summary>
/// Entidade para despesas do usuario
/// </summary>
public class Expense
{

    /// <summary>
    /// Identificador unico da despesa
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Quantia da despesa do usuario
    /// </summary>
    [Required(ErrorMessage = "O Valor da despesa é obrigatório")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Data da despesa adicionada pelo usuario
    /// </summary>
    [Required(ErrorMessage = "A data da despesa é necessária")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Descrição da despesa do usuario. Ex(Aluguel, Mercado, etc..). Não é obrigatório, mas recomendado
    /// </summary>
    [MaxLength(200, ErrorMessage = "A descrição tem que ter no maximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Data da criação da despesa em sistema
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Chave estrangeira com a tabela User
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Propriedade de navegação
    /// </summary>
    public User User { get; set; } = null!;
}
