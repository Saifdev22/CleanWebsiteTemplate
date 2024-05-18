using DomainLayer.DTOs;
using DomainLayer.Responses;

namespace Client.Services
{
    public interface IUserAccountService
    {
        Task<GeneralResponse> CreateAsync(RegisterDTO user);
        Task<LoginResponse> SignInAsync(LoginDTO user);
        //Task<LoginResponse> RefreshTokenAsync(RefreshToken token);
        //Task<WeatherForecast[]> GetWeatherForecasts();

    }
}
