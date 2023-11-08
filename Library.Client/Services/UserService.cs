using Library.Client.Models;
using Library.Common.Exceptions;
using Library.Domain.Dtos.User;
using Library.Domain.Models;
using System.Net.Http;

namespace Library.Client.Services
{
    public class UserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ResponseData<UserViewModel>> GetUser(Guid userId)
        {
            var res = await httpClient.GetAsync($"api/Users/{userId}");

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<UserViewModel> { Data = await res.Content.ReadFromJsonAsync<UserViewModel>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();
                return new ResponseData<UserViewModel> { Data = null, Excetption = exception }; 
            }

        }


    }
}
