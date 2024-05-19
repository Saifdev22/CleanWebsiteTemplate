using Shared.DTOs;

using System.Net.Http.Headers;

namespace Client.Helpers
{
    public class GetHttpClient(IHttpClientFactory httpClientFactory, LocalStorageService localStorageService)
    {
        //GetPrivateHttpClient(Authorization Header Request)
        //GetPublicHttpClient(No Authorization Header Request)

        private const string HeaderKey = "Authorization";

        public async Task<HttpClient> GetPrivateHttpClient()
        {
            //Creates http client request.
            var client = httpClientFactory.CreateClient("SystemApiClient");

            //Extract token from localstorage.
            var stringToken = await localStorageService.GetToken();

            //Validates if token is null or empty.
            if (string.IsNullOrEmpty(stringToken)) return client;

            //If token is not null or not empty we deserialize the token.
            var deserializeToken = Serialization.DeserializeJsonString<GetTokenDTO>(stringToken);

            //Validates the deserialized token.
            if (deserializeToken == null) return client;

            //Finally we add authorization header to the http client request.
            client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", deserializeToken.Token);

            //Returns the client request with authorization header.
            return client;
        }

        public HttpClient GetPublicHttpClient()
        {
            //Creates a http client request.
            var client = httpClientFactory.CreateClient("SystemApiClient");

            //Removes authorization header.
            client.DefaultRequestHeaders.Remove(HeaderKey);

            //Returns the client request without authorization header.
            return client;
        }


    }
}
