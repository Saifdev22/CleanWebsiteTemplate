namespace DomainLayer.Responses
{
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!);
    public record GeneralResponse(bool Flag, string Message = null!);
    public record UserSession(string? Id, string? Name, string? Email, string? Role);
}
