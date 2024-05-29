namespace InfrastructureLayer.IServices
{
    public interface ITokenService
    {
        string CreateToken(CustomUserClaims user);
    }
}
