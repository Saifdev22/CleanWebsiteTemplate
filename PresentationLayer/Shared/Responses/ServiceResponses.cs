namespace Shared.Responses
{
    public record LoginResponse(bool Flag, string Message = null!, string Token = null!);
    public record GeneralResponse(bool Flag, string Message = null!);
    public record CustomUserClaims(string Id = null!, string Name = null!, string Email = null!, string Role = null!);
}
