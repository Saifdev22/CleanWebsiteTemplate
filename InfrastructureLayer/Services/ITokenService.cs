namespace InfrastructureLayer.Services
{
    public interface ITokenService
    {
        string CreateToken(CustomUserClaims user);
    }
}
