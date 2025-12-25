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
}
