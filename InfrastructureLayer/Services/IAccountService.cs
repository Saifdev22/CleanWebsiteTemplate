namespace InfrastructureLayer.Services
{
    public interface IAccountService
    {
        Task<GeneralResponse> RegisterUser(RegisterDTO user);
        Task<LoginResponse> LoginUser(LoginDTO user);
    }
}
