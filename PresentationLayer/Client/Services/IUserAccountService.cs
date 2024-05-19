using Shared.DTOs;
using Shared.Responses;

namespace Client.Services
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> CreateAsync(RegisterDTO user);
        Task<LoginResponse> SignInAsync(LoginDTO user);

    }
}
