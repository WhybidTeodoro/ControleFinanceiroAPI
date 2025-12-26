namespace ControleFinanceiroAPI.DTOs.Monthl;

/// <summary>
/// Dto responsavel por retornar o resumo financeiro mensal do usuario
/// </summary>
public class MonthlyBalanceResponseDto
{
    /// <summary>
    /// Mês de referencia do saldo
    /// </summary>
    public int Month { get; set; }

    /// <summary>
    /// Ano de referencia do saldo
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Total de receita do mês
    /// </summary>
    public decimal TotalIncomes { get; set; }

    /// <summary>
    /// Total de despesas do mês
    /// </summary>
    public decimal TotalExpense { get; set; }

    /// <summary>
    /// Saldo final do mês (Receitas - Despesas)
    /// </summary>
    public decimal Balance { get; set; }
}
