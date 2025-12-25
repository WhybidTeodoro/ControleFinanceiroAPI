namespace ControleFinanceiroAPI.DTOs.Incomes;

/// <summary>
/// Dto responsavel por retornar os dados nos endpoints de Income
/// </summary>
public class IncomeResponseDto
{
    /// <summary>
    /// Identificador unico do usuario no dto reponse
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Representa a renda do usuario no dto response
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Representa a data em que foi adicionada a renda do usuario no dto response
    /// no formato dd/mm/yyyy
    /// </summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>
    /// Representa a descrição da renda do usuario no dto response
    /// </summary>
    public string Description { get; set; } = string.Empty;
}
