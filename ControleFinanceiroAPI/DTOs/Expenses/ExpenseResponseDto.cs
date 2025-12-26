namespace ControleFinanceiroAPI.DTOs.Expenses;

/// <summary>
/// Dto responsavel por retornar os dados necessarios nos endpoints
/// </summary>
public class ExpenseResponseDto
{

    /// <summary>
    /// Identificador unico da despesa no dto de response
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Quantia da despesa no dto de response
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Representa a data em que foi adicionada na despesa do usuario no dto response
    /// no formato dd/mm/yyyy
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Representa a descrição da despesa do usuario no dto response
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
