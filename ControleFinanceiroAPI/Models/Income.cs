using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleFinanceiroAPI.Models;

/// <summary>
/// Classe modelo para as rendas do usuario
/// </summary>
public class Income
{
    /// <summary>
    /// Identifacor unico da renda
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Representa o valor da renda
    /// </summary>
    [Required(ErrorMessage = "O Valor da renda é obrigatório")]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser pelo menos 1 centavo")]
    public decimal Amount { get; set; }

    /// <summary>
    /// representa a data em que o usuario teve aquela renda
    /// </summary>
    [Required(ErrorMessage = "O dia da renda é necessário")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Possibilita a descrição da renda(Ex: Salario, vendas, etc..). Não é obrigatorio mas recomendado
    /// </summary>
    [MaxLength(200, ErrorMessage = "A descrição tem que ter no maximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Representa quando foi criado a renda em sistema
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Chave estrangeira para user
    /// </summary>
    [Required]
    [ForeignKey(nameof(UserId))]
    public int UserId { get; set; }

    /// <summary>
    /// Propriedade de navegação
    /// </summary>
    public User User { get; set; } = null!;

}
