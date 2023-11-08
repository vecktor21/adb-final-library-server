using Library.Client.Models;
using Library.Domain.Dtos;
using Library.Domain.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Library.Client.Services
{
    public class HttpClientService
    {
        private readonly HttpClient client;

        public HttpClientService(HttpClient client)
        {
            this.client = client;
        }


        public async Task<ResponseData<TResponse>> SendPostRequest<TRequest, TResponse>(string action, TRequest requestData)
        {
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            var res = await client.PostAsync(action, content);

            if (res.IsSuccessStatusCode)
            {
                return new ResponseData<TResponse> { Data = await res.Content.ReadFromJsonAsync<TResponse>() };
            }
            else
            {
                var exception = await res.Content.ReadFromJsonAsync<ExceptionResultDto>();

                if(String.IsNullOrEmpty(exception.Message))
                {
                    return new ResponseData<TResponse> 
                    { 
                        Data = default(TResponse), 
                        Excetption = new ExceptionResultDto 
                        { 
                            Code = (int) res.StatusCode,
                            Message = await res.Content.ReadAsStringAsync()
                        } };
                }

                return new ResponseData<TResponse> { Data = default(TResponse), Excetption = exception };
            }
        }
    }
}
