using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;
using Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Client.Helpers
{
    //This class is called everytime you switch pages to check if user is authenticated and authorized on the client side.
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        //GetAuthenticationStateAsync(Executes when a client page is requested)
        //UpdateAuthenticationState(Updates if user tries to login or refresh page)

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //Extract token from localstorage.
            var stringToken = await localStorageService.GetToken();

            //Validates if token is null or empty.
            if (string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

            //If token is not null or not empty we deserialize the token.
            var deserializeToken = Serialization.DeserializeJsonString<GetTokenDTO>(stringToken);

            //Validates the deserialized token.
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymous));

            //Decrypt deserialize token from localstorage.
            var getUserClaims = DecryptToken(deserializeToken.Token!);

            //Validates decrypted token.
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymous));

            //Set claims from decrypted token.
            var claimsPrincipal = SetClaimPrincipal(getUserClaims);

            //Pass claims to AuthenticationState to store.
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }



        public async Task UpdateAuthenticationState(GetTokenDTO getToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (getToken.Token != null)
            {
                var serializeSession = Serialization.SerializeObj(getToken);
                await localStorageService.SetToken(serializeSession);

                var getUserClaims = DecryptToken(getToken.Token!);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, claims.Id!),
                    new(ClaimTypes.Name, claims.Name!),
                    new(ClaimTypes.Email, claims.Email!),
                    new(ClaimTypes.Role, claims.Role!),
                }, "JwtAuth"));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken)) return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);

            return new CustomUserClaims(userId!.Value!, name!.Value, email!.Value, role!.Value);
        }


    }
}
