using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiroAPI.DTOs.Incomes;

/// <summary>
/// Dto Responsavel por criar uma renda para o usuario
/// </summary>
public class CreateIncomeDto
{
    /// <summary>
    /// Representa o valor da renda no dto
    /// </summary>
    [Required(ErrorMessage = "O valor da renda é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser pelo menos 1 centavo")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Representa a data da renda adionada no dto
    /// </summary>
    [Required(ErrorMessage = "O dia da renda é necessário")]
    public DateTime Data { get; set; }

    /// <summary>
    /// Possibilita a descrição da renda(Ex: Salario, vendas, etc..). Não é obrigatorio mas recomendado
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
