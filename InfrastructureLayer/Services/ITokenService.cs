using DomainLayer.Responses;

namespace InfrastructureLayer.Services
{
    public interface ITokenService
    {
        string CreateToken(UserSession user);
    }
}
