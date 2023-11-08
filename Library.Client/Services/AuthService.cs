using Amazon.Runtime.Internal.Transform;
using Library.Client.Models;
using Library.Domain.Dtos;
using Library.Domain.Dtos.User;
using Library.Domain.Models;
using Newtonsoft.Json;
using System.Text;

namespace Library.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient httpClient;
        private readonly HttpClientService httpClientService;

        public AuthService(HttpClient httpClient, HttpClientService httpClientService)
        {
            this.httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public async Task<ResponseData<AuthorizationResponseDto>> Authorize(AuthorizeUserDto auth)
        {
            /*var content = new StringContent(JsonConvert.SerializeObject(auth), Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync($"api/Authorization", content);

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<AuthorizationResponseDto> { Data = await res.Content.ReadFromJsonAsync<AuthorizationResponseDto>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                return new ResponseData<AuthorizationResponseDto> { Data = null, Excetption = exception };
            }*/

            return await httpClientService.SendPostRequest<AuthorizeUserDto, AuthorizationResponseDto>("api/Authorization", auth);
        }


        public async Task<ResponseData<AuthorizationResponseDto>> CheckAuthorization(string accessToken)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "accessToken", accessToken}
            });
            var res = await httpClient.PostAsync($"api/Authorization/CheckAuth", content);

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<AuthorizationResponseDto> { Data = await res.Content.ReadFromJsonAsync<AuthorizationResponseDto>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();
                return new ResponseData<AuthorizationResponseDto> { Data = null, Excetption = exception };
            }
        }
    }
}
