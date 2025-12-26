using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroAPI.DTOs.Expenses;

/// <summary>
/// Dto Responsavel pela criação de despesas do usuario no endpoint
/// </summary>
public class CreateExpenseDto
{
    /// <summary>
    /// Quantia da despesa no dto de criação
    /// </summary>
    [Required(ErrorMessage = "O Valor da despesa é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor tem que ser no minimo 1 centavo")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Data da despesa adicionada pelo usuario no dto de criação
    /// </summary>
    [Required(ErrorMessage = "A data da despesa é necessária")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Descrição da despesa no dto de criação
    /// </summary>
    [MaxLength(200, ErrorMessage = "A descrição tem que ter no maximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;
}
