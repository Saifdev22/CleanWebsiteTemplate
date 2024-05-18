using System.Net;
using Client.Models;

namespace Client.Helpers
{
    public class CustomHttpHandler(LocalStorageService localStorageService) : DelegatingHandler
    {
        //Checks the request for a refresh token, login or register request. And prevents authentication header.
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool loginUrl = request.RequestUri!.AbsoluteUri.Contains("login");
            bool registerUrl = request.RequestUri!.AbsoluteUri.Contains("register");

            if (loginUrl || registerUrl) return await base.SendAsync(request, cancellationToken);

            //Checks if the request has no refresh token, login, register
            var result = await base.SendAsync(request, cancellationToken);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                //Get token from localStorage
                var stringToken = await localStorageService.GetToken();
                if (stringToken == null) return result;

                //Checks for token
                string token = string.Empty;
                try { token = request.Headers.Authorization!.Parameter!; }
                catch { }

                var deserializedToken = Serialization.DeserializeJsonString<UserSession>(stringToken);
                if (deserializedToken is null) return result;

                if (string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", deserializedToken.Token);
                    return await base.SendAsync(request, cancellationToken);
                }
            }

            return result;
        }
    }
}
