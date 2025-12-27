using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiroAPI.Models;

/// <summary>
/// Entidade para as rendas do usuario
/// </summary>
public class Income
{
    /// <summary>
    /// Identifacor unico da renda
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Quantia da renda do usuario
    /// </summary>
    [Required(ErrorMessage = "O Valor da renda é obrigatório")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser pelo menos 1 centavo")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Data da renda adicionada pelo usuario
    /// </summary>
    [Required(ErrorMessage = "A data da renda é necessária")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Descrição da renda do usuario(Ex: Salario, vendas, etc..). Não é obrigatorio mas recomendado
    /// </summary>
    [MaxLength(200, ErrorMessage = "A descrição tem que ter no maximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Data da criação da renda em sistema
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indica se a renda é recorrente
    /// </summary>
    public bool IsRecurring { get; set; } = false;

    /// <summary>
    /// Dia fixo do mes para recorrencia(1 - 31)
    /// Só deve ser preenchido se IsRecurring = true
    /// </summary>
    [Range(1, 31, ErrorMessage = "O Dia do mes deve estar entre 1 e 31")]
    public int? DayOfMonth { get; set; }

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
