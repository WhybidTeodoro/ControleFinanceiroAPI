namespace ControleFinanceiroAPI.DTOs;

/// <summary>
/// DTO para registro de usuario no endpoint
/// </summary>
public class RegisterUserDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
