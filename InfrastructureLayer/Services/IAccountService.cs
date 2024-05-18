using DomainLayer.DTOs;
using DomainLayer.Responses;

namespace InfrastructureLayer.Services
{
    public interface IAccountService
    {
        Task<GeneralResponse> RegisterUser(RegisterDTO user);
        Task<LoginResponse> LoginUser(LoginDTO user);

    }
}
