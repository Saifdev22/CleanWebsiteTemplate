using Client.Helpers;
using Shared.DTOs;
using Shared.Responses;
using System.Net.Http.Json;


namespace Client.Services
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/account";

        public async Task<GeneralResponse> CreateAsync(RegisterDTO user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/register", user);
            if (!result.IsSuccessStatusCode) return new GeneralResponse(false, " Error occured");

            return await result.Content.ReadFromJsonAsync<GeneralResponse>();
        }

        public async Task<LoginResponse> SignInAsync(LoginDTO user)
        {
            var httpClient = getHttpClient.GetPublicHttpClient();
            var result = await httpClient.PostAsJsonAsync($"{AuthUrl}/login", user);

            if (!result.IsSuccessStatusCode) return new LoginResponse(false, " Error occured");

            return await result.Content.ReadFromJsonAsync<LoginResponse>();
        }

    }
}
