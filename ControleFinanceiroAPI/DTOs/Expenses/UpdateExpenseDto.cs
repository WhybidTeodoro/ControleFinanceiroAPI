using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroAPI.DTOs.Expenses;

/// <summary>
/// Dto Responsavel por atualizar alguma despesa do usuario no endpoint
/// </summary>
public class UpdateExpenseDto
{
    /// <summary>
    /// Quantia da despesa no dto de atualização
    /// </summary>
    [Required(ErrorMessage = "O Valor da despesa é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor tem que ser no minimo 1 centavo")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Data da despesa adicionada pelo usuario no dto de atualização
    /// </summary>
    [Required(ErrorMessage = "A data da despesa é necessária")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Descrição da despesa no dto de atualização
    /// </summary>
    [MaxLength(200, ErrorMessage = "A descrição tem que ter no maximo 200 caracteres")]
    public string Description { get; set; } = string.Empty;

    // <summary>
    /// Indica se a renda é recorrente
    /// </summary>
    public bool IsRecurring { get; set; } = false;

    /// <summary>
    /// Dia fixo do mes para recorrencia(1 - 31)
    /// Só deve ser preenchido se IsRecurring = true
    /// </summary>
    [Range(1, 31, ErrorMessage = "O Dia do mes deve estar entre 1 e 31")]
    public int? DayOfMonth { get; set; }
}
